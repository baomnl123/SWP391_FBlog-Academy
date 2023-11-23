using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using System.Linq;

namespace backend.Handlers.Implementors
{
    public class PostHandlers : IPostHandlers
    {
        private readonly IFollowUserRepository _followUserRepository;
        private readonly IReportPostRepository _reportPostRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageHandlers _imageHandlers;
        private readonly IVideoHandlers _videoHandlers;
        private readonly ISubjectHandlers _subjectHandlers;
        private readonly IMajorHandlers _majorHandlers;
        private readonly IPostSubjectRepository _postSubjectRepository;
        private readonly IPostMajorRepository _postMajorRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IUserSubjectRepository _userSubjectRepository;
        private readonly IUserMajorRepository _userMajorRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly SpecialMajors _specialMajors;

        public PostHandlers(IPostRepository postRepository,
                            IUserRepository userRepository,
                            IImageHandlers imageHandlers,
                            IVideoHandlers videoHandlers,
                            ISubjectHandlers subjectHandlers,
                            IMajorHandlers majorHandlers,
                            IPostSubjectRepository postSubjectRepository,
                            IPostMajorRepository postMajorRepository,
                            IVotePostRepository votePostRepository,
                            IMajorRepository majorRepository,
                            IUserSubjectRepository userSubjectRepository,
                            IUserMajorRepository userMajorRepository,
                            IMapper mapper,
                            IFollowUserRepository followUserRepository,
                            IReportPostRepository reportPostRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _imageHandlers = imageHandlers;
            _videoHandlers = videoHandlers;
            _subjectHandlers = subjectHandlers;
            _majorHandlers = majorHandlers;
            _postSubjectRepository = postSubjectRepository;
            _postMajorRepository = postMajorRepository;
            _votePostRepository = votePostRepository;
            _majorRepository = majorRepository;
            _userSubjectRepository = userSubjectRepository;
            _userMajorRepository = userMajorRepository;
            _userRoleConstrant = new UserRoleConstrant();
            _specialMajors = new SpecialMajors();
            _followUserRepository = followUserRepository;
            _reportPostRepository = reportPostRepository;
        }

        public PostDTO? ApprovePost(int reviewerId, int postId)
        {
            //check reviewer is not null
            //                  and not yet removed
            //                  and has role MOD(Moderator) or LT(Lecturer)
            var reviewer = _userRepository.GetUser(reviewerId);
            var modRole = _userRoleConstrant.GetModeratorRole();
            var lecRole = _userRoleConstrant.GetLecturerRole();

            if (reviewer != null
                && reviewer.Status == true
                && (reviewer.Role.Contains(modRole) || reviewer.Role.Contains(lecRole)))
            {
                //check post needed approved exists
                var existedPost = _postRepository.GetPost(postId);

                //check post is null or removed or approved
                if (existedPost == null
                    || existedPost.Status == false
                    || (existedPost.Status == true && existedPost.IsApproved == true)) return null;

                //update info of existedPost
                existedPost.ReviewerId = reviewerId;
                existedPost.IsApproved = true;
                existedPost.UpdatedAt = DateTime.Now;

                //Mapping existedPost to data type PostDTO which have more fields (Videos, Images, subjects, majors)
                var approvingPost = _mapper.Map<PostDTO>(existedPost);
                //return null if mapping is failed
                if (approvingPost is null) return null;

                var user = _userRepository.GetUserByPostID(approvingPost.Id);
                if (user == null || !user.Status) return null;
                approvingPost.User = _mapper.Map<UserDTO>(user);

                var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(approvingPost.Id));
                approvingPost.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(approvingPost.Id));
                approvingPost.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                var getImages = _imageHandlers.GetImagesByPost(approvingPost.Id);
                approvingPost.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(approvingPost.Id);
                approvingPost.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(approvingPost.Id);

                var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                approvingPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                approvingPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(approvingPost.Id);

                var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                approvingPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                approvingPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                //Update info to database
                if (!_postRepository.UpdatePost(existedPost)) return null;
                return approvingPost;
            }

            //return null if reviewer is invalid
            return null;
        }

        public PostDTO? CreatePost(int userId, string title, string content,
                                                    int[]? subjectIds, int[]? majorIds,
                                                    string[]? imageURLs, string[]? videoURLs)
        {
            //return null if creating post is failed
            var createdPost = CreatePost(userId, title, content);
            if (createdPost == null) return null;

            //attach user for post
            if (AttachUserForPost(createdPost, userId) is null) return null;

            //create Images for post if it is necessary
            if (imageURLs is not null && imageURLs.Length > 0)
            {
                var images = _imageHandlers.CreateImage(createdPost.Id, imageURLs);
                if (images is null || images.Count == 0)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Images = images;
            }
            else createdPost.Images = new List<ImageDTO>();

            //create Videos for post if it is necessary
            if (videoURLs is not null && videoURLs.Length > 0)
            {
                var videos = _videoHandlers.CreateVideo(createdPost.Id, videoURLs);
                if (videos is null || videos.Count == 0)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Videos = videos;
            }
            else createdPost.Videos = new List<VideoDTO>();

            //add majors for post if it is necessary
            if (majorIds is not null && majorIds.Length > 0)
            {
                var majors = AttachMajorsForPost(createdPost, majorIds);
                if (majors is null || majors.Count == 0) return null;
                createdPost.Majors = majors;
            }
            else createdPost.Majors = new List<MajorDTO>();

            //add subjects for post if it is successful, return null otherwise
            if (subjectIds is not null && subjectIds.Length > 0)
            {
                var subjects = AttachSubjectsForPost(createdPost, subjectIds);
                if (subjects is null || subjects.Count == 0) return null;
                createdPost.Subjects = subjects;
            }
            else createdPost.Subjects = new List<SubjectDTO>();

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(createdPost.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            createdPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            createdPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(createdPost.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            createdPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            createdPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            return createdPost;
        }

        public ICollection<SubjectDTO>? AttachSubjectsForPost(PostDTO createdPost, int[] subjectIds)
        {
            var subjects = new List<SubjectDTO>();

            foreach (var subjectId in subjectIds)
            {
                var subject = _subjectHandlers.GetSubjectById(subjectId);

                if (subject == null || !subject.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                var addedSubject = _subjectHandlers.CreatePostSubject(createdPost, subject);

                if (addedSubject == null || !addedSubject.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                subjects.Add(addedSubject);
            }

            return subjects;
        }

        public ICollection<MajorDTO>? AttachMajorsForPost(PostDTO createdPost, int[] majorIds)
        {
            //create a majors' list to return
            List<MajorDTO> majors = new();
            foreach (var majorId in majorIds)
            {
                //Undo creating post and return null if major does not exist or is removed
                var major = _majorHandlers.GetMajorById(majorId);
                if (major is null || !major.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                };

                //Undo creating post and return null if relationship of post and major does not exist or is removed
                var addedMajor = _majorHandlers.CreatePostMajor(createdPost, major);
                if (addedMajor is null || !addedMajor.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                //add major to majors' list if creating reltionship is successful
                majors.Add(addedMajor);
            }

            return majors;
        }

        public UserDTO? AttachUserForPost(PostDTO createdPost, int userID)
        {
            var user = _userRepository.GetUser(userID);
            if (user == null || !user.Status)
            {
                return null;
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            createdPost.User = userDTO;
            return userDTO;
        }

        public PostDTO? CreatePost(int userId, string title, string content)
        {
            //check info is not null
            if (title is null || content is null)
            {
                return null;
            }

            //check if userId is not existed
            //                or removed
            var existedUser = _userRepository.GetUser(userId);
            if (existedUser == null || existedUser.Status == false) return null;

            //create new post
            Post newPost = new()
            {
                UserId = userId,
                Title = title,
                Content = content,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                Status = true,
            };

            //add new post to database
            if (!_postRepository.CreateNewPost(newPost)) return null;

            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            var lecturerRole = _userRoleConstrant.GetLecturerRole();

            //approve post if post's owner is award
            if (existedUser.IsAwarded == true || existedUser.Role.Contains(moderatorRole) || existedUser.Role.Contains(lecturerRole))
            {
                //update info of createdPost
                newPost.IsApproved = true;

                //Update info to database
                if (!_postRepository.UpdatePost(newPost)) return null;
            }

            //add newPostSubject ralationship to database
            return _mapper.Map<PostDTO>(newPost);
        }

        public PostDTO? Delete(int postId)
        {
            //return null if post which needs to delete does not exist or is removed
            var deletedPost = _postRepository.GetPost(postId);
            if (deletedPost == null
                || deletedPost.Status == false) return null;

            //set Status to false (it means deleted)
            deletedPost.Status = false;
            deletedPost.UpdatedAt = DateTime.Now;

            //return null if deleting post is failed.Otherwise, return deleted post with data type PostDTO 
            if (!_postRepository.UpdatePost(deletedPost)) return null;
            return _mapper.Map<PostDTO>(deletedPost);
        }

        public PostDTO? DisablePost(int postId)
        {
            //return null if post which needs to delete does not exist or is removed
            var deletedPost = _postRepository.GetPost(postId);
            if (deletedPost == null
                || deletedPost.Status == false) return null;

            //set Status to false (it means deleted)
            deletedPost.Status = false;
            deletedPost.UpdatedAt = DateTime.Now;

            //Mapping deletedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
            var deletingPost = _mapper.Map<PostDTO>(deletedPost);
            //return null if mapping is failed
            if (deletingPost is null) return null;

            //Get user who owns post
            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(deletingPost.Id));
            deletingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            //return null if disable all votes of post is failed
            if (!_votePostRepository.DisableAllVotePostOf(deletedPost)) return null;

            //return null if disabling all data related to post is failed
            var successDisabled = DisableAllRelatedToPost(deletingPost);
            if (successDisabled == null) return null;

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(successDisabled.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            successDisabled.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            successDisabled.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(successDisabled.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            successDisabled.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            successDisabled.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            //return null if deleting post is failed.Otherwise, return deleted post with data type PostDTO 
            if (!_postRepository.UpdatePost(deletedPost)) return null;
            return _mapper.Map<PostDTO>(deletedPost);
        }

        public PostDTO? DisableAllRelatedToPost(PostDTO deletingPost)
        {
            //disable Images of post if post has Medias
            var deletingImages = _imageHandlers.GetImagesByPost(deletingPost.Id);
            if (deletingImages is not null && deletingImages.Count > 0)
            {
                foreach (var image in deletingImages)
                {
                    var successDelete = _imageHandlers.DisableImage(image.Id);
                    if (successDelete is null) return null;
                }
            }
            deletingPost.Images = new List<ImageDTO>();

            //disable Videos of post if post has Medias
            var deletingVideos = _videoHandlers.GetVideosByPost(deletingPost.Id);
            if (deletingVideos is not null && deletingVideos.Count > 0)
            {
                foreach (var video in deletingVideos)
                {
                    var successDelete = _videoHandlers.DisableVideo(video.Id);
                    if (successDelete is null) return null;
                }
            }
            deletingPost.Videos = new List<VideoDTO>();

            //disable subjects of post if post has subjects
            var subjectsOfPost = _postSubjectRepository.GetPostSubjectsByPostId(deletingPost.Id);
            foreach (var subject in subjectsOfPost)
            {
                var disabledSubject = _subjectHandlers.DisablePostSubject(deletingPost.Id, subject.SubjectId);
                if (disabledSubject is null) return null;
            }
            deletingPost.Subjects = new List<SubjectDTO>();

            //disable majors of post if post has majors
            var majorsOfPost = _postMajorRepository.GetPostMajorsByPostId(deletingPost.Id);
            foreach (var major in majorsOfPost)
            {
                var disabledMajor = _majorHandlers.DisablePostMajor(deletingPost.Id, major.MajorId);
                if (disabledMajor is null) return null;
            }
            deletingPost.Majors = new List<MajorDTO>();

            return deletingPost;
        }

        public PostDTO? DenyPost(int reviewerId, int postId)
        {
            //return null if validReviewer is null
            //                              or removed
            //                              or does not have role MOD(Moderator) or LT(Lecturer)
            var validReviewer = _userRepository.GetUser(reviewerId);
            var modRole = _userRoleConstrant.GetModeratorRole();
            var lecRole = _userRoleConstrant.GetLecturerRole();
            if (validReviewer == null
                || validReviewer.Status == false
                || !(validReviewer.Role.Contains(modRole) || validReviewer.Role.Contains(lecRole))) return null;

            //return null if existedPost is null
            //                              or removed
            //                              or approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null
                || existedPost.Status == false
                || (existedPost.Status == true && existedPost.IsApproved == true)) return null;

            //update info of existedPost which is denied
            existedPost.ReviewerId = reviewerId;
            existedPost.IsApproved = false;
            existedPost.Status = false;
            existedPost.UpdatedAt = DateTime.Now;

            //Mapping existedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
            var disablingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (disablingPost is null) return null;

            //Get user who owns post
            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(disablingPost.Id));
            disablingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            //return null if disable all votes of post is failed
            if (!_votePostRepository.DisableAllVotePostOf(existedPost)) return null;

            //return null if disabling all data related to post is failed
            var successDisabled = DisableAllRelatedToPost(disablingPost);
            if (successDisabled == null) return null;

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(successDisabled.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            successDisabled.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            successDisabled.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(successDisabled.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            successDisabled.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            successDisabled.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            //update info to database
            if (!_postRepository.UpdatePost(existedPost)) return null;
            return successDisabled;
        }

        public ICollection<PostDTO>? GetAllPosts(int currentUserId)
        {
            //return null if get all posts is failed
            var existed = _postRepository.GetAllPosts();
            if (existed == null || existed.Count == 0) return null;

            //return posts'list
            return GetPostInformationByRole(existed, currentUserId);
        }

        public ICollection<PostDTO>? GetAllPostsForAdmin(int currentUserId)
        {
            //return null if get all posts is failed
            var existed = _postRepository.GetAllPosts();
            if (existed == null || existed.Count == 0) return null;

            //return posts'list
            return GetAllRelatedDataForPost(existed);
        }

        public ICollection<PostDTO>? SearchPostByUserId(int userId)
        {
            //check that user is not null
            //                  or removed
            //                  or that user does not have role SU(Student) or role MOD(Moderator)
            //return null if search Posts' list of userId is failed
            var existedPostList = _postRepository.SearchPostByUserId(userId);
            if (existedPostList == null || existedPostList.Count == 0) return null;

            //return posts'list
            return GetAllRelatedDataForPost(existedPostList);
        }

        public ICollection<PostDTO>? SearchPostsByTitle(string title, int currentUserId)
        {
            //Search all posts which contain content
            var existed = _postRepository.SearchPostsByTitle(title);
            if (existed == null || existed.Count == 0) return null;

            //return posts'list
            return GetPostInformationByRole(existed, currentUserId);
        }

        public PostDTO? UpdatePost(int postId, string title, string content,
                                                int[]? subjectIds, int[]? majorIds,
                                                string[]? imageURLs, string[]? videoURLs)
        {
            //check post is existed
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status == true) return null;

            //Update info of existed post
            existedPost.Title = title;
            existedPost.Content = content;
            existedPost.UpdatedAt = DateTime.Now;

            //Mapping existedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
            var updatingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (updatingPost is null) return null;

            //Get user who owns post
            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(updatingPost.Id));
            updatingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            //updating Images if it is successful, return null otherwise 
            if (imageURLs is not null && imageURLs.Length > 0)
            {
                var updatedImages = UpdateImagesOfPost(postId, imageURLs);
                if (updatedImages is null) return null;
                updatingPost.Images = updatedImages;
            }
            else updatingPost.Images = new List<ImageDTO>();

            //Updating Videos if it is successful.Otherwise, return null
            if (videoURLs is not null && videoURLs.Length > 0)
            {
                var updatedVideos = UpdateVideosOfPost(postId, videoURLs);
                if (updatedVideos is null) return null;
                updatingPost.Videos = updatedVideos;
            }
            else updatingPost.Videos = new List<VideoDTO>();

            if (subjectIds is not null && subjectIds.Length > 0)
            {
                //Disable all relationship of post and subjects to update new
                var subjectsOfPost = _postSubjectRepository.GetPostSubjectsByPostId(postId);
                foreach (var subject in subjectsOfPost)
                {
                    var disabledSubject = _subjectHandlers.DisablePostSubject(postId, subject.SubjectId);
                    if (disabledSubject is null) return null;
                }

                //update subjects for post if it is successful, return null otherwise
                var subjects = AttachSubjectsForPost(updatingPost, subjectIds);
                if (subjects is null || subjects.Count == 0) return null;
                updatingPost.Subjects = subjects;
            }
            else updatingPost.Subjects = new List<SubjectDTO>();

            if (majorIds is not null && majorIds.Length > 0)
            {
                //Disable all relationship of post and majors to update new
                var majorsOfPost = _postMajorRepository.GetPostMajorsByPostId(postId);
                foreach (var major in majorsOfPost)
                {
                    var disabledMajor = _majorHandlers.DisablePostMajor(postId, major.MajorId);
                    if (disabledMajor is null) return null;
                }

                //update majors for post if it is successful, return null otherwise
                var majors = AttachMajorsForPost(updatingPost, majorIds);
                if (majors is null || majors.Count == 0) return null;
                updatingPost.Majors = majors;
            }
            else updatingPost.Majors = new List<MajorDTO>();

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(updatingPost.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            updatingPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            updatingPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(updatingPost.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            updatingPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            updatingPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            //Update to database
            if (!_postRepository.UpdatePost(existedPost)) return null;
            return updatingPost;
        }

        public ICollection<ImageDTO>? UpdateImagesOfPost(int postId, string[] imageURLs)
        {
            //disable Medias of post if post has Medias
            var images = _imageHandlers.GetImagesByPost(postId);
            if (images is not null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var successDelete = _imageHandlers.DisableImage(image.Id);
                    if (successDelete is null) return null;
                }
            }

            //update Medias
            var updatedImages = _imageHandlers.CreateImage(postId, imageURLs);
            if (updatedImages is null || updatedImages.Count == 0) return new List<ImageDTO>();
            else return updatedImages;
        }

        public ICollection<VideoDTO>? UpdateVideosOfPost(int postId, string[] videoURLs)
        {
            //disable Videos of post if post has Videos
            var videos = _videoHandlers.GetVideosByPost(postId);
            if (videos is not null && videos.Count > 0)
            {
                foreach (var video in videos)
                {
                    var successDelete = _videoHandlers.DisableVideo(video.Id);
                    if (successDelete is null) return null;
                }
            }

            //update Videos
            var updatedVideos = _videoHandlers.CreateVideo(postId, videoURLs);
            if (updatedVideos is null || updatedVideos.Count == 0) return new List<VideoDTO>();
            else return updatedVideos;
        }

        public ICollection<PostDTO>? ViewPendingPostList(int currentUserId)
        {
            //check valid viewer
            var validViewer = _userRepository.GetUser(currentUserId);
            var modRole = _userRoleConstrant.GetModeratorRole();
            var lecRole = _userRoleConstrant.GetLecturerRole();
            if (validViewer is null || !validViewer.Status
                || !(validViewer.Role.Contains(modRole) || validViewer.Role.Contains(lecRole))) return null;

            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList();
            if (existedList == null || existedList.Count == 0) return null;

            //return posts'list
            var tempList = GetAllRelatedDataForPost(existedList);
            var returnlist = new List<PostDTO>();

            //Get majors of currentUser
            var subjects = _userSubjectRepository.GetSubjectsOf(validViewer.Id);
            if (subjects is null || subjects.Count == 0) return null;

            foreach (var postDTO in tempList)
            {
                if (postDTO.Subjects == null || postDTO.Subjects.Count == 0) continue;

                ICollection<SubjectDTO> subjectsDTO = _mapper.Map<ICollection<SubjectDTO>>(subjects);
                ICollection<SubjectDTO> subjectPostDTO = postDTO.Subjects.Where(x => x is not null).ToList();
                var findCommon = subjectPostDTO.Join(subjectsDTO,
                    x => x.Id, y => y.Id, (x, y) => x);

                if (findCommon.Any())
                    returnlist.Add(postDTO);
            }

            returnlist = returnlist.DistinctBy(p => p.Id).ToList();
            return returnlist;
        }

        public ICollection<PostDTO>? ViewPendingPostListOf(int userId)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList(userId);
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = new List<PostDTO>();
            //get related data for all post
            resultList = GetAllRelatedDataForPost(existedList);

            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? ViewDeletedPostOf(int userId)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewDeletedPost(userId);
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = new List<PostDTO>();
            //get related data for all post
            resultList = GetAllRelatedDataForPost(existedList);

            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? GetAllPosts(int[] majorIDs, int[] subjectIDs, string searchValue, int currentUserId)
        {
            //if both majors and subjects are empty getallposts.
            if ((majorIDs == null || majorIDs.Length == 0)
                && (subjectIDs == null || subjectIDs.Length == 0)
                && (searchValue == null || searchValue.Equals(string.Empty))
                && (currentUserId == null || currentUserId == 0))
            {
                return null;
            }
            if ((majorIDs == null || majorIDs.Length == 0)
                && (subjectIDs == null || subjectIDs.Length == 0)
                && (searchValue == null || searchValue.Equals(string.Empty)))
            {
                return GetAllPostsOnLoad(currentUserId);
            }
            if ((majorIDs == null || majorIDs.Length == 0)
                && (subjectIDs == null || subjectIDs.Length == 0))
            {
                return SearchPostsByTitle(searchValue, currentUserId);
            }

            //get post list based on majoriesIDs and subjectIDs
            var postList = _postRepository.GetPost(majorIDs, subjectIDs);
            //check if null
            if (postList == null || postList.Count == 0)
            {
                return null;
            }
            //init new List
            List<PostDTO> resultList = new List<PostDTO>();
            resultList = GetPostInformationByRole(postList, currentUserId);


            if (searchValue != null)
            {
                if (!searchValue.Equals(string.Empty))
                {
                    for (int i = resultList.Count - 1; i >= 0; i--)
                    {
                        var post = resultList.ElementAt(i);
                        if (!post.Title.ToLower().Contains(searchValue.ToLower())
                            && !post.Content.ToLower().Contains(searchValue.ToLower()))
                            resultList.Remove(post);
                    }
                }
            }

            if (resultList.Count == 0) return null;

            return resultList;
        }

        public ICollection<PostDTO>? GetAllPostsOnLoad(int currentUserId)
        {
            //check valid viewer
            var validViewer = _userRepository.GetUser(currentUserId);
            if (validViewer is null || !validViewer.Status) return null;

            var majors = _userMajorRepository.GetMajorsOf(validViewer.Id);
            var subjects = _userSubjectRepository.GetSubjectsOf(validViewer.Id);

            //if both majors and subjects are empty getallposts.
            if ((majors == null || majors.Count == 0)
                && (subjects == null || subjects.Count == 0)
                && (currentUserId == null || currentUserId == 0))
            {
                return null;
            }
            if ((majors == null || majors.Count == 0)
                && (subjects == null || subjects.Count == 0))
            {
                return GetAllPosts(currentUserId);
            }
            if ((subjects == null || subjects.Count == 0))
            {
                var returnList = GetPostWithMajor(currentUserId, majors);
                if (returnList is null || returnList.Count == 0) return new List<PostDTO>();
                return returnList;
            }

            //return null if get pending posts' list is failed
            var existedList = _postRepository.GetAllPosts();
            if (existedList == null || existedList.Count == 0) return null;

            //return posts'list
            var tempList = GetPostInformationByRole(existedList, currentUserId);
            var returnlist = new List<PostDTO>();

            //Get majors of currentUser
            if (subjects is null || subjects.Count == 0) return null;

            foreach (var postDTO in tempList)
            {
                if (postDTO.Subjects == null || postDTO.Subjects.Count == 0) continue;

                ICollection<SubjectDTO> subjectsDTO = _mapper.Map<ICollection<SubjectDTO>>(subjects);
                ICollection<SubjectDTO> subjectPostDTO = postDTO.Subjects.Where(x => x is not null).ToList();
                var findCommon = subjectPostDTO.Join(subjectsDTO,
                    x => x.Id, y => y.Id, (x, y) => x);

                if (findCommon.Any())
                    returnlist.Add(postDTO);
            }

            returnlist = returnlist.DistinctBy(p => p.Id).ToList();
            return returnlist;
        }

        public ICollection<PostDTO>? GetPostWithMajor(int currentUserId, ICollection<Major> majors)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.GetAllPosts();
            if (existedList == null || existedList.Count == 0) return null;

            //return posts'list
            var tempList = GetPostInformationByRole(existedList, currentUserId);
            var returnlist = new List<PostDTO>();

            //Get majors of currentUser
            if (majors is null || majors.Count == 0) return null;

            foreach (var postDTO in tempList)
            {
                if (postDTO.Majors == null || postDTO.Majors.Count == 0) continue;

                ICollection<MajorDTO> majorsDTO = _mapper.Map<ICollection<MajorDTO>>(majors);
                ICollection<MajorDTO> majorPostDTO = postDTO.Majors.Where(x => x is not null).ToList();
                var findCommon = majorPostDTO.Join(majorsDTO,
                    x => x.Id, y => y.Id, (x, y) => x);

                if (findCommon.Any())
                    returnlist.Add(postDTO);
            }

            returnlist = returnlist.DistinctBy(p => p.Id).ToList();
            return returnlist;
        }

        public PostDTO? GetPostBy(int postId, int currentUserId)
        {
            //Get post by post's id
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //Mapping existedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
            var existingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (existingPost is null) return null;

            var currentUser = _userRepository.GetUser(currentUserId);
            if (currentUser == null || !currentUser.Status) return null;
            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(existingPost.Id));
            if (getUser == null || !getUser.Status) return null;

            //get users follow relationship
            var postOwner = _userRepository.GetUser(getUser.Id);

            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, postOwner);
            if (followRelationship != null)
            {
                if (followRelationship.Status) getUser.isFollowed = true;
            }
            existingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(existingPost.Id));
            existingPost.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

            var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(existingPost.Id));
            existingPost.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

            var getImages = _imageHandlers.GetImagesByPost(existingPost.Id);
            existingPost.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

            var getVideos = _videoHandlers.GetVideosByPost(existingPost.Id);
            existingPost.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(existingPost.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

            //get users follow relationship
            if (UsersUpvote != null)
            {
                if (UsersUpvote.Count > 0)
                {
                    foreach (var userDTO in UsersUpvote)
                    {
                        var user = _userRepository.GetUser(userDTO.Id);

                        if (user == null || !user.Status) continue;
                        followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);

                        if (followRelationship != null)
                        {
                            if (followRelationship.Status) userDTO.isFollowed = true;
                        }

                    }
                }
            }

            existingPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            existingPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(existingPost.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);

            //get users follow relationship
            if (UsersDownvote != null)
            {
                if (UsersDownvote.Count > 0)
                {
                    foreach (var userDTO in UsersDownvote)
                    {
                        var user = _userRepository.GetUser(userDTO.Id);

                        if (user == null || !user.Status) continue;
                        followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);

                        if (followRelationship != null)
                        {
                            if (followRelationship.Status) userDTO.isFollowed = true;
                        }

                    }
                }
            }

            existingPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            existingPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            var validViewer = CheckCurrentUser(currentUserId);
            if (validViewer != null)
            {
                var vote = _votePostRepository.GetVotePost(currentUserId, existingPost.Id);
                if (vote != null) existingPost.Vote = vote.Vote;
            }
            return existingPost;
        }

        public PostDTO? GetPostBy(int postId)
        {
            //Get post by post's id
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //Mapping existedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
            var existingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (existingPost is null) return null;

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(existingPost.Id));
            existingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(existingPost.Id));
            existingPost.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

            var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(existingPost.Id));
            existingPost.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

            var getImages = _imageHandlers.GetImagesByPost(existingPost.Id);
            existingPost.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

            var getVideos = _videoHandlers.GetVideosByPost(existingPost.Id);
            existingPost.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(existingPost.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            existingPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            existingPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(existingPost.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            existingPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            existingPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            var getReports = _reportPostRepository.GetAllReportsAboutPost(existingPost.Id);
            if (getReports != null)
            {
                var reportPostsDTO = new List<ReportPostDTO>();
                foreach (var report in getReports)
                {
                    var reportDTO = _mapper.Map<ReportPostDTO>(report);
                    var getReporterUser = _userRepository.GetUser(report.ReporterId);
                    var getReportedPost = this.GetPostBy(report.PostId);
                    if (getReporterUser == null || !getReporterUser.Status) continue;
                    if (getReportedPost == null || !getReportedPost.Status) continue;
                    reportDTO.Reporter = _mapper.Map<UserDTO>(getReporterUser);
                    reportDTO.Post = getReportedPost;
                    reportPostsDTO.Add(reportDTO);
                }
                existingPost.Reports = getReports.Count();
            }

            return existingPost;
        }

        public ICollection<PostDTO>? GetPostsHaveImage(int currentUserId)
        {
            var posts = GetAllPosts(currentUserId);

            return posts?.Where(p => p.Images?.Any() == true).ToList();
        }

        public ICollection<PostDTO>? GetPostsHaveVideo(int currentUserId)
        {
            var posts = GetAllPosts(currentUserId);

            return posts?.Where(p => p.Videos?.Any() == true).ToList();
        }

        public UserDTO? CheckCurrentUser(int currentUserId)
        {
            var user = _userRepository.GetUser(currentUserId);
            if (user == null || !user.Status) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public ICollection<PostDTO>? GetTop5VotedPost(int currentUserId)
        {
            var postList = GetAllPosts(currentUserId);
            if (postList == null || postList.Count == 0)
            {
                return null;
            }

            var hashMap = new Dictionary<PostDTO, float>();
            foreach (var postDTO in postList)
            {
                DateTime currentDate = DateTime.Now;

                DateTime date31DaysAgo = currentDate.AddDays(-31);

                TimeSpan daySpan = DateTime.Now - date31DaysAgo;
                int daysDiff = daySpan.Days;

                int countUpvotes = postDTO.Upvotes.HasValue ? postDTO.Upvotes.Value : 0;
                hashMap.Add(postDTO, (float) countUpvotes / daysDiff);
            }

            var sortedKeyValuePairs = hashMap.OrderByDescending(p => p.Value).Take(5);
            postList = sortedKeyValuePairs.Select(p => p.Key).ToList();

            return postList;
        }

        private List<PostDTO> GetAllRelatedDataForPost(ICollection<Post> existed)
        {
            List<PostDTO> returnList = new List<PostDTO>();

            foreach (var post in existed)
            {
                if (!post.Status) continue;

                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                if (getUser == null || !getUser.Status) continue;

                var postDTO = _mapper.Map<PostDTO>(post);

                postDTO.User = getUser;

                var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                var getImages = _imageHandlers.GetImagesByPost(postDTO.Id);
                postDTO.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(postDTO.Id);
                postDTO.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                postDTO.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                postDTO.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(postDTO.Id);

                var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                var getReports = _reportPostRepository.GetAllReportsAboutPost(postDTO.Id);
                if (getReports != null)
                {
                    var reportPostsDTO = new List<ReportPostDTO>();
                    foreach (var report in getReports)
                    {
                        var reportDTO = _mapper.Map<ReportPostDTO>(report);
                        var getReporterUser = _userRepository.GetUser(report.ReporterId);
                        var getReportedPost = this.GetPostBy(report.PostId);
                        if (getReporterUser == null || !getReporterUser.Status) continue;
                        if (getReportedPost == null || !getReportedPost.Status) continue;
                        reportDTO.Reporter = _mapper.Map<UserDTO>(getReporterUser);
                        reportDTO.Post = getReportedPost;
                        reportPostsDTO.Add(reportDTO);
                    }
                    postDTO.ReportList = reportPostsDTO;
                    postDTO.Reports = getReports.Count();
                }

                returnList.Add(postDTO);
            }

            return returnList;
        }

        private List<PostDTO> GetPostInformationByRole(ICollection<Post> existed, int currentUserId)
        {
            List<PostDTO> returnList = new List<PostDTO>();

            var onlyStudentMajor = _majorRepository.GetMajorByName(_specialMajors.GetOnlyStudent());
            var validViewer = _userRepository.GetUser(currentUserId);

            if (validViewer == null || !validViewer.Status)
                return GetAllRelatedDataForPost(existed);

            if (validViewer.Role.Contains(_userRoleConstrant.GetLecturerRole()))
            {
                var currentUser = _mapper.Map<UserDTO>(validViewer);

                //get related data for all post
                foreach (var post in existed)
                {
                    if (!post.Status) continue;

                    var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(post.Id));
                    if (getMajors.Any(item => item.Id == onlyStudentMajor.Id)) continue;

                    var getUser = _userRepository.GetUserByPostID(post.Id);
                    if (getUser == null || !getUser.Status) continue;

                    var userDTO = _mapper.Map<UserDTO>(getUser);
                    var postDTO = _mapper.Map<PostDTO>(post);

                    //get users follow relationship


                    var followRelationship = _followUserRepository.GetFollowRelationship(validViewer, getUser);
                    if (followRelationship != null)
                    {
                        if (followRelationship.Status) userDTO.isFollowed = true;
                    }
                    postDTO.User = (userDTO is not null && userDTO.Status) ? userDTO : null;

                    postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                    var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                    postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                    var getImages = _imageHandlers.GetImagesByPost(postDTO.Id);
                    postDTO.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                    var getVideos = _videoHandlers.GetVideosByPost(postDTO.Id);
                    postDTO.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                    var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                    var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                    //get users follow relationship
                    if (UsersUpvote != null)
                    {
                        if (UsersUpvote.Count > 0)
                        {
                            foreach (var userUpvoteDTO in UsersUpvote)
                            {
                                var userUpvote = _userRepository.GetUser(userUpvoteDTO.Id);
                                if (userUpvote == null || !userUpvote.Status) continue;

                                followRelationship = _followUserRepository.GetFollowRelationship(validViewer, userUpvote);
                                if (followRelationship != null)
                                {
                                    if (followRelationship.Status) userUpvoteDTO.isFollowed = true;
                                }
                            }
                        }
                    }

                    postDTO.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                    postDTO.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                    var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(postDTO.Id);

                    var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);

                    //get users follow relationship
                    if (UsersDownvote != null)
                    {
                        if (UsersDownvote.Count > 0)
                        {
                            foreach (var userDownvoteDTO in UsersDownvote)
                            {
                                var userDownvote = _userRepository.GetUser(userDownvoteDTO.Id);
                                if (userDownvote == null || !userDownvote.Status) continue;
                                followRelationship = _followUserRepository.GetFollowRelationship(validViewer, userDownvote);
                                if (followRelationship != null)
                                {
                                    if (followRelationship.Status) userDownvoteDTO.isFollowed = true;
                                }
                            }
                        }
                    }

                    postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                    postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                    var getReports = _reportPostRepository.GetAllReportsAboutPost(postDTO.Id);
                    if (getReports != null)
                    {
                        var reportPostsDTO = new List<ReportPostDTO>();
                        foreach (var report in getReports)
                        {
                            var reportDTO = _mapper.Map<ReportPostDTO>(report);
                            var getReporterUser = _userRepository.GetUser(report.ReporterId);
                            var getReportedPost = this.GetPostBy(report.PostId);
                            if (getReporterUser == null || !getReporterUser.Status) continue;
                            if (getReportedPost == null || !getReportedPost.Status) continue;
                            reportDTO.Reporter = _mapper.Map<UserDTO>(getReporterUser);
                            reportDTO.Post = getReportedPost;
                            reportPostsDTO.Add(reportDTO);
                        }
                        postDTO.ReportList = reportPostsDTO;
                        postDTO.Reports = getReports.Count();
                    }

                    if (validViewer != null)
                    {
                        var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                        if (vote != null)
                        {
                            postDTO.Vote = vote.Vote;
                        }
                    }
                    returnList.Add(postDTO);
                }
            }
            else
            {
                var currentUser = _mapper.Map<UserDTO>(validViewer);

                //get related data for all post
                foreach (var post in existed)
                {
                    if (!post.Status) continue;

                    var getUser = _userRepository.GetUserByPostID(post.Id);
                    if (getUser == null || !getUser.Status) continue;

                    var userDTO = _mapper.Map<UserDTO>(getUser);
                    var postDTO = _mapper.Map<PostDTO>(post);

                    //get users follow relationship
                    var followRelationship = _followUserRepository.GetFollowRelationship(validViewer, getUser);
                    if (followRelationship != null)
                    {
                        if (followRelationship.Status) userDTO.isFollowed = true;
                    }
                    postDTO.User = (userDTO is not null && userDTO.Status) ? userDTO : null;

                    var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(post.Id));
                    postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                    var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                    postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                    var getImages = _imageHandlers.GetImagesByPost(postDTO.Id);
                    postDTO.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                    var getVideos = _videoHandlers.GetVideosByPost(postDTO.Id);
                    postDTO.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                    var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                    var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                    //get users follow relationship
                    if (UsersUpvote != null)
                    {
                        if (UsersUpvote.Count > 0)
                        {
                            foreach (var userUpvoteDTO in UsersUpvote)
                            {
                                var userUpvote = _userRepository.GetUser(userUpvoteDTO.Id);
                                if (userUpvote == null || !userUpvote.Status) continue;

                                followRelationship = _followUserRepository.GetFollowRelationship(validViewer, userUpvote);
                                if (followRelationship != null)
                                {
                                    if (followRelationship.Status) userUpvoteDTO.isFollowed = true;
                                }
                            }
                        }
                    }

                    postDTO.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                    postDTO.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                    var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(postDTO.Id);

                    var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);

                    //get users follow relationship
                    if (UsersDownvote != null)
                    {
                        if (UsersDownvote.Count > 0)
                        {
                            foreach (var userDownvoteDTO in UsersDownvote)
                            {
                                var userDownvote = _userRepository.GetUser(userDownvoteDTO.Id);
                                if (userDownvote == null || !userDownvote.Status) continue;
                                followRelationship = _followUserRepository.GetFollowRelationship(validViewer, userDownvote);
                                if (followRelationship != null)
                                {
                                    if (followRelationship.Status) userDownvoteDTO.isFollowed = true;
                                }
                            }
                        }
                    }

                    postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                    postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                    var getReports = _reportPostRepository.GetAllReportsAboutPost(postDTO.Id);
                    if (getReports != null)
                    {
                        var reportPostsDTO = new List<ReportPostDTO>();
                        foreach(var report in getReports)
                        {
                            var reportDTO = _mapper.Map<ReportPostDTO>(report);
                            var getReporterUser = _userRepository.GetUser(report.ReporterId);
                            var getReportedPost = this.GetPostBy(report.PostId);
                            if (getReporterUser == null || !getReporterUser.Status) continue;
                            if (getReportedPost == null || !getReportedPost.Status) continue;
                            reportDTO.Reporter = _mapper.Map<UserDTO>(getReporterUser);
                            reportDTO.Post = getReportedPost;
                            reportPostsDTO.Add(reportDTO);
                        }
                        postDTO.ReportList = reportPostsDTO;
                        postDTO.Reports = getReports.Count();
                    }

                    if (validViewer != null)
                    {
                        var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                        if (vote != null)
                        {
                            postDTO.Vote = vote.Vote;
                        }
                    }
                    returnList.Add(postDTO);
                }
            }

            return returnList;
        }
    }
}
