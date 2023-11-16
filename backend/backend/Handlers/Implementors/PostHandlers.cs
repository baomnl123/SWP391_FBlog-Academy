using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;

namespace backend.Handlers.Implementors
{
    public class PostHandlers : IPostHandlers
    {
        private readonly IFollowUserRepository _followUserRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediaHandlers _MediaHandlers;
        private readonly ISubjectHandlers _subjectHandlers;
        private readonly IMajorHandlers _majorHandlers;
        private readonly IPostSubjectRepository _postSubjectRepository;
        private readonly IPostMajorRepository _postMajorRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly SpecialMajors _specialMajors;

        public PostHandlers(IPostRepository postRepository,
                            IUserRepository userRepository,
                            IMediaHandlers MediaHandlers,
                            ISubjectHandlers subjectHandlers,
                            IMajorHandlers majorHandlers,
                            IPostSubjectRepository postSubjectRepository,
                            IPostMajorRepository postMajorRepository,
                            IVotePostRepository votePostRepository,
                            IMajorRepository majorRepository,
                            IMapper mapper,
                            IFollowUserRepository followUserRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _MediaHandlers = MediaHandlers;
            _subjectHandlers = subjectHandlers;
            _majorHandlers = majorHandlers;
            _postSubjectRepository = postSubjectRepository;
            _postMajorRepository = postMajorRepository;
            _votePostRepository = votePostRepository;
            _majorRepository = majorRepository;
            _userRoleConstrant = new UserRoleConstrant();
            _specialMajors = new SpecialMajors();
            _followUserRepository = followUserRepository;
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

                //Mapping existedPost to data type PostDTO which have more fields (Medias, Medias, subjects, majors)
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

                var getMedias = _MediaHandlers.GetMediasByPost(approvingPost.Id);
                approvingPost.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

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
                                                    string[]? MediaURLs)
        {
            //return null if creating post is failed
            var createdPost = CreatePost(userId, title, content);
            if (createdPost == null) return null;

            //attach user for post
            if (AttachUserForPost(createdPost, userId) is null)
            {
                return null;
            }

            //create Medias for post if it is necessary
            if (MediaURLs is not null && MediaURLs.Length > 0)
            {
                var Medias = _MediaHandlers.CreateMedia(createdPost.Id, MediaURLs);
                if (Medias is null || Medias.Count == 0)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Medias = Medias;
            }
            else createdPost.Medias = new List<MediaDTO>();

            //create Medias for post if it is necessary
            if (MediaURLs is not null && MediaURLs.Length > 0)
            {
                var Medias = _MediaHandlers.CreateMedia(createdPost.Id, MediaURLs);
                if (Medias is null || Medias.Count == 0)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Medias = Medias;
            }
            else createdPost.Medias = new List<MediaDTO>();

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
            //create a subjects' list to return
            List<SubjectDTO> subjects = new();

            foreach (var subjectId in subjectIds)
            {
                //Undo creating post and return null if subject does not exist or is removed
                var subject = _subjectHandlers.GetSubjectById(subjectId);
                if (subject is null || !subject.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                };

                //Undo creating post and return null if relationship of post and subject does not exist or is removed
                var addedSubject = _subjectHandlers.CreatePostSubject(createdPost, subject);
                if (addedSubject is null || !addedSubject.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                //add subject to subjects' list if creating reltionship is successful
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
            //disable Medias of post if post has Medias
            var deletingMedias = _MediaHandlers.GetMediasByPost(deletingPost.Id);
            if (deletingMedias is not null && deletingMedias.Count > 0)
            {
                foreach (var Media in deletingMedias)
                {
                    var successDelete = _MediaHandlers.DisableMedia(Media.Id);
                    if (successDelete is null) return null;
                }
            }
            deletingPost.Medias = new List<MediaDTO>();

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

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existed);
            var onlyStudentMajor = _majorRepository.GetMajorByName(_specialMajors.GetOnlyStudent());
            var validViewer = CheckCurrentUser(currentUserId);

            if (validViewer == null || !validViewer.Status)
            {
                foreach (var post in resultList)
                {
                    if (post.Status)
                    {
                        var postDTO = _mapper.Map<PostDTO>(post);

                        var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));
                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                        postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                        postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                        var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                        postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                        postDTO.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                        postDTO.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                        var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(postDTO.Id);

                        var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                        postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                        postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                    }

                }
            }

            if (validViewer.Role.Contains(_userRoleConstrant.GetLecturerRole()))
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                //get related data for all post
                for (int i = resultList.Count - 1; i >= 0; i--)
                {
                    if (resultList[i].Status)
                    {
                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(resultList[i].Id));
                        if (!getMajors.Any(item => item.Id == onlyStudentMajor.Id))
                        {
                            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(resultList[i].Id));

                            //get users follow relationship
                            if (getUser != null)
                            {
                                if (getUser.Status)
                                {
                                    var user = _userRepository.GetUser(getUser.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                            if (followRelationship != null)
                                            {
                                                if (followRelationship.Status)
                                                {
                                                    getUser.isFollowed = true;
                                                }
                                            }
                                            resultList[i].User = (getUser is not null && getUser.Status) ? getUser : null;

                                            resultList[i].Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                            var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(resultList[i].Id));
                                            resultList[i].Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                            var getMedias = _MediaHandlers.GetMediasByPost(resultList[i].Id);
                                            resultList[i].Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                            var postUpvote = _votePostRepository.GetAllUsersVotedBy(resultList[i].Id);

                                            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                            //get users follow relationship
                                            if (UsersUpvote != null)
                                            {
                                                if (UsersUpvote.Count > 0)
                                                {
                                                    foreach (var userDTO in UsersUpvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            resultList[i].UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                                            resultList[i].Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                                            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(resultList[i].Id);

                                            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);

                                            //get users follow relationship
                                            if (UsersDownvote != null)
                                            {
                                                if (UsersDownvote.Count > 0)
                                                {
                                                    foreach (var userDTO in UsersDownvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            resultList[i].UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                            resultList[i].Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                                            if (validViewer != null)
                                            {
                                                var vote = _votePostRepository.GetVotePost(currentUserId, resultList[i].Id);
                                                if (vote != null)
                                                {
                                                    resultList[i].Vote = vote.Vote;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            resultList.Remove(resultList[i]);
                                        }
                                    }
                                    else
                                    {
                                        resultList.Remove(resultList[i]);
                                    }
                                }
                                else
                                {
                                    resultList.Remove(resultList[i]);
                                }
                            }
                            else
                            {
                                resultList.Remove(resultList[i]);
                            }
                        }
                        else
                        {
                            resultList.Remove(resultList[i]);
                        }
                    }
                    else
                    {
                        resultList.Remove(resultList[i]);
                    }
                }
            }
            else
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                for (int i = resultList.Count - 1; i >= 0; i--)
                {
                    if (resultList[i].Status)
                    {
                        var postDTO = _mapper.Map<PostDTO>(resultList[i]);

                        var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));

                        //get users follow relationship
                        if (getUser != null)
                        {
                            if (getUser.Status)
                            {
                                var user = _userRepository.GetUser(getUser.Id);
                                if (user != null)
                                {
                                    if (user.Status)
                                    {
                                        var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                        if (followRelationship != null)
                                        {
                                            if (followRelationship.Status)
                                            {
                                                getUser.isFollowed = true;
                                            }
                                        }
                                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                                        postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                                        postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                        var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                                        postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                        //get users follow relationship
                                        if (UsersUpvote != null)
                                        {
                                            if (UsersUpvote.Count > 0)
                                            {
                                                foreach (var userDTO in UsersUpvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
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
                                                foreach (var userDTO in UsersDownvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                        postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;


                                        if (validViewer != null)
                                        {
                                            var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                                            if (vote != null)
                                            {
                                                postDTO.Vote = vote.Vote;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultList.Remove(resultList[i]);
                                    }
                                }
                                else
                                {
                                    resultList.Remove(resultList[i]);
                                }
                            }
                            else
                            {
                                resultList.Remove(resultList[i]);
                            }
                        }
                        else
                        {
                            resultList.Remove(resultList[i]);
                        }
                    }
                    else
                    {
                        resultList.Remove(resultList[i]);
                    }
                }
            }
            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? SearchPostByUserId(int userId)
        {
            //check that user is not null
            //                  or removed
            //                  or that user does not have role SU(Student) or role MOD(Moderator)
            var exitedUser = _userRepository.GetUser(userId);
            //var studentRole = _userRoleConstrant.GetStudentRole();
            //var modRole = _userRoleConstrant.GetModeratorRole();
            //if (exitedUser == null
            //    || exitedUser.Status == false
            //    || !(exitedUser.Role.Contains(studentRole) || exitedUser.Role.Contains(modRole))) return null;

            //return null if search Posts' list of userId is failed
            var existedPostList = _postRepository.SearchPostByUserId(userId);
            if (existedPostList == null || existedPostList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedPostList);

            //get related data for all post
            for (int i = resultList.Count - 1; i >= 0; i--)
            {
                if (resultList[i].Status)
                {
                    var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(resultList[i].Id));
                    if (getUser?.Status == true)
                    {
                        resultList[i].User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(resultList[i].Id));
                        resultList[i].Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(resultList[i].Id));
                        resultList[i].Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                        var getMedias = _MediaHandlers.GetMediasByPost(resultList[i].Id);
                        resultList[i].Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(resultList[i].Id);

                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                        resultList[i].UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                        resultList[i].Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                        var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(resultList[i].Id);

                        var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                        resultList[i].UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                        resultList[i].Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                        var validViewer = CheckCurrentUser(userId);
                        if (validViewer != null)
                        {
                            var vote = _votePostRepository.GetVotePost(userId, resultList[i].Id);
                            if (vote != null)
                            {
                                resultList[i].Vote = vote.Vote;
                            }
                        }
                    }
                    else
                    {
                        resultList.Remove(resultList[i]);
                    }
                }
                else
                {
                    resultList.Remove(resultList[i]);
                }
            }
            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? SearchPostsByTitle(string title, int currentUserId)
        {
            //Search all posts which contain content
            var list = _postRepository.SearchPostsByTitle(title);
            if (list == null || list.Count == 0) return null;

            //map to list DTO
            var resultList = new List<PostDTO>();
            var onlyStudentMajor = _majorRepository.GetMajorByName(_specialMajors.GetOnlyStudent());
            var validViewer = CheckCurrentUser(currentUserId);
            if (validViewer.Role.Contains(_userRoleConstrant.GetLecturerRole()))
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                foreach (var post in list)
                {
                    if (post.Status)
                    {

                        //init postDTO
                        var postDTO = _mapper.Map<PostDTO>(post);

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                        if (!getMajors.Any(item => item.Id == onlyStudentMajor.Id))
                        {
                            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));

                            //get users follow relationship
                            if (getUser != null)
                            {
                                if (getUser.Status)
                                {
                                    var user = _userRepository.GetUser(getUser.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                            if (followRelationship != null)
                                            {
                                                if (followRelationship.Status)
                                                {
                                                    getUser.isFollowed = true;
                                                }
                                            }
                                            postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                                            postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                            var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                                            postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                            var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                                            postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                            var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                                            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                            //get users follow relationship
                                            if (UsersUpvote != null)
                                            {
                                                if (UsersUpvote.Count > 0)
                                                {
                                                    foreach (var userDTO in UsersUpvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
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
                                                    foreach (var userDTO in UsersDownvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                            postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                                            if (validViewer != null)
                                            {
                                                var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                                                if (vote != null)
                                                {
                                                    postDTO.Vote = vote.Vote;
                                                }
                                            }
                                            resultList.Add(postDTO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                foreach (var post in list)
                {
                    if (post.Status)
                    {
                        var postDTO = _mapper.Map<PostDTO>(post);

                        var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));

                        //get users follow relationship
                        if (getUser != null)
                        {
                            if (getUser.Status)
                            {
                                var user = _userRepository.GetUser(getUser.Id);
                                if (user != null)
                                {
                                    if (user.Status)
                                    {
                                        var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                        if (followRelationship != null)
                                        {
                                            if (followRelationship.Status)
                                            {
                                                getUser.isFollowed = true;
                                            }
                                        }
                                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                                        postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                                        postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                        var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                                        postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                        //get users follow relationship
                                        if (UsersUpvote != null)
                                        {
                                            if (UsersUpvote.Count > 0)
                                            {
                                                foreach (var userDTO in UsersUpvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
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
                                                foreach (var userDTO in UsersDownvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                        postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;


                                        if (validViewer != null)
                                        {
                                            var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                                            if (vote != null)
                                            {
                                                postDTO.Vote = vote.Vote;
                                            }
                                        }
                                        resultList.Add(postDTO);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            //get related data for all post


            //return posts'list
            return resultList;
        }

        public PostDTO? UpdatePost(int postId, string title, string content,
                                                int[]? subjectIds, int[]? majorIds,
                                                string[]? MediaURLs)
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

            //updating Medias if it is successful, return null otherwise 
            if (MediaURLs is not null && MediaURLs.Length > 0)
            {
                var updatedMedias = UpdateMediasOfPost(postId, MediaURLs);
                if (updatedMedias is null) return null;
                updatingPost.Medias = updatedMedias;
            }
            else updatingPost.Medias = new List<MediaDTO>();

            //Updating Medias if it is successful.Otherwise, return null
            if (MediaURLs is not null && MediaURLs.Length > 0)
            {
                var updatedMedias = UpdateMediasOfPost(postId, MediaURLs);
                if (updatedMedias is null) return null;
                updatingPost.Medias = updatedMedias;
            }
            else updatingPost.Medias = new List<MediaDTO>();

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

        public ICollection<MediaDTO>? UpdateMediasOfPost(int postId, string[] MediaURLs)
        {
            //disable Medias of post if post has Medias
            var Medias = _MediaHandlers.GetMediasByPost(postId);
            if (Medias is not null && Medias.Count > 0)
            {
                foreach (var Media in Medias)
                {
                    var successDelete = _MediaHandlers.DisableMedia(Media.Id);
                    if (successDelete is null) return null;
                }
            }

            //update Medias
            var updatedMedias = _MediaHandlers.CreateMedia(postId, MediaURLs);
            if (updatedMedias is null || updatedMedias.Count == 0) return new List<MediaDTO>();
            else return updatedMedias;
        }

        public ICollection<PostDTO>? ViewPendingPostList()
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList();
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedList);

            //get related data for all post
            for (int i = resultList.Count - 1; i >= 0; i--)
            {
                if (resultList[i].Status)
                {
                    var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(resultList[i].Id));
                    if (getUser?.Status == true)
                    {
                        resultList[i].User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(resultList[i].Id));
                        resultList[i].Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(resultList[i].Id));
                        resultList[i].Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                        var getMedias = _MediaHandlers.GetMediasByPost(resultList[i].Id);
                        resultList[i].Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(resultList[i].Id);

                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                        resultList[i].UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                        resultList[i].Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                        var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(resultList[i].Id);

                        var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                        resultList[i].UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                        resultList[i].Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;
                    }
                    else
                    {
                        resultList.Remove(resultList[i]);
                    }
                }
                else
                {
                    resultList.Remove(resultList[i]);
                }
            }

            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? ViewPendingPostListOf(int userId)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList(userId);
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedList);

            //get related data for all post
            for (int i = resultList.Count - 1; i >= 0; i--)
            {
                if (resultList[i].Status)
                {
                    var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(resultList[i].Id));
                    if (getUser?.Status == true)
                    {
                        resultList[i].User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(resultList[i].Id));
                        resultList[i].Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(resultList[i].Id));
                        resultList[i].Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                        var getMedias = _MediaHandlers.GetMediasByPost(resultList[i].Id);
                        resultList[i].Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(resultList[i].Id);

                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                        resultList[i].UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                        resultList[i].Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                        var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(resultList[i].Id);

                        var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                        resultList[i].UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                        resultList[i].Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;
                    }
                    else
                    {
                        resultList.Remove(resultList[i]);
                    }
                }
                else
                {
                    resultList.Remove(resultList[i]);
                }
            }

            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? ViewDeletedPostOf(int userId)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewDeletedPost(userId);
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedList);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(post.Id));
                post.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(post.Id));
                post.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                var getMedias = _MediaHandlers.GetMediasByPost(post.Id);
                post.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(post.Id);

                var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                post.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                post.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(post.Id);

                var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                post.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                post.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;
            }

            //return posts'list
            return resultList;
        }

        public ICollection<PostDTO>? GetAllPosts(int[] majorIDs, int[] subjectIDs, string searchValue, int currentUserId)
        {
            //if both majors and subjects are empty getallposts.
            if ((majorIDs == null || majorIDs.Length == 0) && (subjectIDs == null || subjectIDs.Length == 0) && (searchValue == null || searchValue.Equals(string.Empty)) && (currentUserId == null || currentUserId == 0))
            {
                return null;
            }
            if ((majorIDs == null || majorIDs.Length == 0) && (subjectIDs == null || subjectIDs.Length == 0) && (searchValue == null || searchValue.Equals(string.Empty)))
            {
                return GetAllPosts(currentUserId);
            }
            if ((majorIDs == null || majorIDs.Length == 0) && (subjectIDs == null || subjectIDs.Length == 0))
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
            var postListDTO = new List<PostDTO>();

            var onlyStudentMajor = _majorRepository.GetMajorByName(_specialMajors.GetOnlyStudent());
            var validViewer = CheckCurrentUser(currentUserId);

            if (validViewer == null || !validViewer.Status)
            {
                return null;
            }

            if (validViewer.Role.Contains(_userRoleConstrant.GetLecturerRole()))
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                foreach (var post in postList)
                {
                    if (post.Status)
                    {
                        //init postDTO
                        var postDTO = _mapper.Map<PostDTO>(post);

                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                        if (!getMajors.Any(item => item.Id == onlyStudentMajor.Id))
                        {
                            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));

                            //get users follow relationship
                            if (getUser != null)
                            {
                                if (getUser.Status)
                                {
                                    var user = _userRepository.GetUser(getUser.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                            if (followRelationship != null)
                                            {
                                                if (followRelationship.Status)
                                                {
                                                    getUser.isFollowed = true;
                                                }
                                            }
                                            postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                                            postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                            var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                                            postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                            var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                                            postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                            var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                                            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                            //get users follow relationship
                                            if (UsersUpvote != null)
                                            {
                                                if (UsersUpvote.Count > 0)
                                                {
                                                    foreach (var userDTO in UsersUpvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
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
                                                    foreach (var userDTO in UsersDownvote)
                                                    {
                                                        user = _userRepository.GetUser(userDTO.Id);
                                                        if (user != null)
                                                        {
                                                            if (user.Status)
                                                            {
                                                                followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                                if (followRelationship != null)
                                                                {
                                                                    if (followRelationship.Status)
                                                                    {
                                                                        userDTO.isFollowed = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                            postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                                            if (validViewer != null)
                                            {
                                                var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                                                if (vote != null)
                                                {
                                                    postDTO.Vote = vote.Vote;
                                                }
                                            }
                                            postListDTO.Add(postDTO);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var currentUser = _userRepository.GetUser(currentUserId);
                if (currentUser == null || !currentUser.Status)
                {
                    return null;
                }
                foreach (var post in postList)
                {

                    if (post.Status)
                    {
                        //init postDTO
                        var postDTO = _mapper.Map<PostDTO>(post);

                        var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(postDTO.Id));

                        //get users follow relationship
                        if (getUser != null)
                        {
                            if (getUser.Status)
                            {
                                var user = _userRepository.GetUser(getUser.Id);
                                if (user != null)
                                {
                                    if (user.Status)
                                    {
                                        var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                        if (followRelationship != null)
                                        {
                                            if (followRelationship.Status)
                                            {
                                                getUser.isFollowed = true;
                                            }
                                        }
                                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                                        var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(postDTO.Id));
                                        postDTO.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                        var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(postDTO.Id));
                                        postDTO.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                        var getMedias = _MediaHandlers.GetMediasByPost(postDTO.Id);
                                        postDTO.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(postDTO.Id);

                                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                        //get users follow relationship
                                        if (UsersUpvote != null)
                                        {
                                            if (UsersUpvote.Count > 0)
                                            {
                                                foreach (var userDTO in UsersUpvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
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
                                                foreach (var userDTO in UsersDownvote)
                                                {
                                                    user = _userRepository.GetUser(userDTO.Id);
                                                    if (user != null)
                                                    {
                                                        if (user.Status)
                                                        {
                                                            followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                            if (followRelationship != null)
                                                            {
                                                                if (followRelationship.Status)
                                                                {
                                                                    userDTO.isFollowed = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        postDTO.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                                        postDTO.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                                        if (validViewer != null)
                                        {
                                            var vote = _votePostRepository.GetVotePost(currentUserId, postDTO.Id);
                                            if (vote != null)
                                            {
                                                postDTO.Vote = vote.Vote;
                                            }
                                        }
                                        postListDTO.Add(postDTO);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (searchValue != null)
            {
                if (!searchValue.Equals(string.Empty))
                {

                    for (int i = postListDTO.Count - 1; i >= 0; i--)
                    {
                        var post = postListDTO.ElementAt(i);
                        if (!post.Title.ToLower().Contains(searchValue.ToLower()) && !post.Content.ToLower().Contains(searchValue.ToLower()))
                        {
                            postListDTO.Remove(post);
                        }
                    }
                }
            }

            if (postListDTO.Count == 0)
            {
                return null;
            }

            return postListDTO;
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
            if (currentUser != null)
            {
                if (currentUser.Status)
                {
                    var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(existingPost.Id));

                    //get users follow relationship
                    if (getUser != null)
                    {
                        if (getUser.Status)
                        {
                            var user = _userRepository.GetUser(getUser.Id);
                            if (user != null)
                            {
                                if (user.Status)
                                {
                                    var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                    if (followRelationship != null)
                                    {
                                        if (followRelationship.Status)
                                        {
                                            getUser.isFollowed = true;
                                        }
                                    }
                                    existingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

                                    var getMajors = _mapper.Map<ICollection<MajorDTO>?>(_postMajorRepository.GetMajorsOf(existingPost.Id));
                                    existingPost.Majors = (getMajors is not null && getMajors.Count > 0) ? getMajors : new List<MajorDTO>();

                                    var getSubjects = _mapper.Map<ICollection<SubjectDTO>?>(_postSubjectRepository.GetSubjectsOf(existingPost.Id));
                                    existingPost.Subjects = (getSubjects is not null && getSubjects.Count > 0) ? getSubjects : new List<SubjectDTO>();

                                    var getMedias = _MediaHandlers.GetMediasByPost(existingPost.Id);
                                    existingPost.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

                                    var postUpvote = _votePostRepository.GetAllUsersVotedBy(existingPost.Id);

                                    var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                                    //get users follow relationship
                                    if (UsersUpvote != null)
                                    {
                                        if (UsersUpvote.Count > 0)
                                        {
                                            foreach (var userDTO in UsersUpvote)
                                            {
                                                user = _userRepository.GetUser(userDTO.Id);
                                                if (user != null)
                                                {
                                                    if (user.Status)
                                                    {
                                                        followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
                                                        if (followRelationship != null)
                                                        {
                                                            if (followRelationship.Status)
                                                            {
                                                                userDTO.isFollowed = true;
                                                            }
                                                        }
                                                    }
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
                                                user = _userRepository.GetUser(userDTO.Id);
                                                if (user != null)
                                                {
                                                    if (user.Status)
                                                    {
                                                        followRelationship = _followUserRepository.GetFollowRelationship(user, currentUser);
                                                        if (followRelationship != null)
                                                        {
                                                            if (followRelationship.Status)
                                                            {
                                                                userDTO.isFollowed = true;
                                                            }
                                                        }
                                                    }
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
                                        if (vote != null)
                                        {
                                            existingPost.Vote = vote.Vote;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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

            var getMedias = _MediaHandlers.GetMediasByPost(existingPost.Id);
            existingPost.Medias = (getMedias is not null && getMedias.Count > 0) ? getMedias : new List<MediaDTO>();

            var postUpvote = _votePostRepository.GetAllUsersVotedBy(existingPost.Id);

            var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
            existingPost.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
            existingPost.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

            var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(existingPost.Id);

            var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
            existingPost.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
            existingPost.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

            return existingPost;
        }

        public ICollection<PostDTO>? GetPostsHaveMedia(int currentUserId)
        {
            var posts = GetAllPosts(currentUserId);

            return posts?.Where(p => p.Medias?.Any() == true).ToList();
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

            return postList
              .OrderByDescending(p => p.Upvotes)
              .Take(5)
              .ToList();
        }
    }
}
