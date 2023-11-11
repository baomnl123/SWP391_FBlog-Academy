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
        private readonly IVideoHandlers _videoHandlers;
        private readonly IImageHandlers _imageHandlers;
        private readonly ITagHandlers _tagHandlers;
        private readonly ICategoryHandlers _categoryHandlers;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly SpecialCategories _specialCategories;

        public PostHandlers(IPostRepository postRepository,
                            IUserRepository userRepository,
                            IVideoHandlers videoHandlers,
                            IImageHandlers imageHandlers,
                            ITagHandlers tagHandlers,
                            ICategoryHandlers categoryHandlers,
                            IPostTagRepository postTagRepository,
                            IPostCategoryRepository postCategoryRepository,
                            IVotePostRepository votePostRepository,
                            ICategoryRepository categoryRepository,
                            IMapper mapper,
                            IFollowUserRepository followUserRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _videoHandlers = videoHandlers;
            _imageHandlers = imageHandlers;
            _tagHandlers = tagHandlers;
            _categoryHandlers = categoryHandlers;
            _postTagRepository = postTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _votePostRepository = votePostRepository;
            _categoryRepository = categoryRepository;
            _userRoleConstrant = new UserRoleConstrant();
            _specialCategories = new SpecialCategories();
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

                //Mapping existedPost to data type PostDTO which have more fields (videos, images, tags, categories)
                var approvingPost = _mapper.Map<PostDTO>(existedPost);
                //return null if mapping is failed
                if (approvingPost is null) return null;

                var user = _userRepository.GetUserByPostID(approvingPost.Id);
                if (user == null || !user.Status) return null;
                approvingPost.User = _mapper.Map<UserDTO>(user);

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(approvingPost.Id));
                approvingPost.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(approvingPost.Id));
                approvingPost.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                                    int[]? tagIds, int[]? categoryIds,
                                                    string[]? videoURLs, string[]? imageURLs)
        {
            //return null if creating post is failed
            var createdPost = CreatePost(userId, title, content);
            if (createdPost == null) return null;

            //attach user for post
            if (AttachUserForPost(createdPost, userId) is null)
            {
                return null;
            }

            //create videos for post if it is necessary
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

            //create images for post if it is necessary
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

            //add categories for post if it is necessary
            if (categoryIds is not null && categoryIds.Length > 0)
            {
                var categories = AttachCategoriesForPost(createdPost, categoryIds);
                if (categories is null || categories.Count == 0) return null;
                createdPost.Categories = categories;
            }
            else createdPost.Categories = new List<CategoryDTO>();

            //add tags for post if it is successful, return null otherwise
            if (tagIds is not null && tagIds.Length > 0)
            {
                var tags = AttachTagsForPost(createdPost, tagIds);
                if (tags is null || tags.Count == 0) return null;
                createdPost.Tags = tags;
            }
            else createdPost.Tags = new List<TagDTO>();

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

        public ICollection<TagDTO>? AttachTagsForPost(PostDTO createdPost, int[] tagIds)
        {
            //create a tags' list to return
            List<TagDTO> tags = new();

            foreach (var tagId in tagIds)
            {
                //Undo creating post and return null if tag does not exist or is removed
                var tag = _tagHandlers.GetTagById(tagId);
                if (tag is null || !tag.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                };

                //Undo creating post and return null if relationship of post and tag does not exist or is removed
                var addedTag = _tagHandlers.CreatePostTag(createdPost, tag);
                if (addedTag is null || !addedTag.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                //add tag to tags' list if creating reltionship is successful
                tags.Add(addedTag);
            }

            return tags;
        }

        public ICollection<CategoryDTO>? AttachCategoriesForPost(PostDTO createdPost, int[] categoryIds)
        {
            //create a categories' list to return
            List<CategoryDTO> categories = new();
            foreach (var categoryId in categoryIds)
            {
                //Undo creating post and return null if category does not exist or is removed
                var category = _categoryHandlers.GetCategoryById(categoryId);
                if (category is null || !category.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                };

                //Undo creating post and return null if relationship of post and category does not exist or is removed
                var addedCategory = _categoryHandlers.CreatePostCategory(createdPost, category);
                if (addedCategory is null || !addedCategory.Status)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                //add category to categories' list if creating reltionship is successful
                categories.Add(addedCategory);
            }

            return categories;
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

            //approve post if post's owner is award
            if (existedUser.IsAwarded == true)
            {
                //update info of createdPost
                newPost.IsApproved = true;

                //Update info to database
                if (!_postRepository.UpdatePost(newPost)) return null;
            }

            //add newPostTag ralationship to database
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

            //Mapping deletedPost to data type PostDTO which have more fields (videos, images, tags, categories)
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
            //disable videos of post if post has videos
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

            //disable images of post if post has images
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

            //disable tags of post if post has tags
            var tagsOfPost = _postTagRepository.GetPostTagsByPostId(deletingPost.Id);
            foreach (var tag in tagsOfPost)
            {
                var disabledTag = _tagHandlers.DisablePostTag(deletingPost.Id, tag.TagId);
                if (disabledTag is null) return null;
            }
            deletingPost.Tags = new List<TagDTO>();

            //disable categories of post if post has categories
            var categoriesOfPost = _postCategoryRepository.GetPostCategoriesByPostId(deletingPost.Id);
            foreach (var category in categoriesOfPost)
            {
                var disabledCategory = _categoryHandlers.DisablePostCategory(deletingPost.Id, category.CategoryId);
                if (disabledCategory is null) return null;
            }
            deletingPost.Categories = new List<CategoryDTO>();

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

            //Mapping existedPost to data type PostDTO which have more fields (videos, images, tags, categories)
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
            return _mapper.Map<PostDTO>(existedPost);
        }

        public ICollection<PostDTO>? GetAllPosts(int currentUserId)
        {
            //return null if get all posts is failed
            var existed = _postRepository.GetAllPosts();
            if (existed == null || existed.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existed);
            var onlyStudentCategory = _categoryRepository.GetCategoryByName(_specialCategories.GetOnlyStudent());
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

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                        var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                        postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                foreach (var post in resultList)
                {
                    var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                    if (!getCategories.Any(item => item.Id == onlyStudentCategory.Id))
                    {
                        var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));

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
                                    }

                                }
                            }
                        }

                        post.User = (getUser is not null && getUser.Status) ? getUser : null;

                        post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                        var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                        post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                        var getImages = _imageHandlers.GetImagesByPost(post.Id);
                        post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                        var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                        post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                        var postUpvote = _votePostRepository.GetAllUsersVotedBy(post.Id);

                        var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);

                        //get users follow relationship
                        if (UsersUpvote != null)
                        {
                            if (UsersUpvote.Count > 0)
                            {
                                foreach (var userDTO in UsersUpvote)
                                {
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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

                        post.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                        post.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                        var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(post.Id);

                        var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);

                        //get users follow relationship
                        if (UsersDownvote != null)
                        {
                            if (UsersDownvote.Count > 0)
                            {
                                foreach (var userDTO in UsersDownvote)
                                {
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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

                        post.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                        post.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;


                        if (validViewer != null)
                        {
                            var vote = _votePostRepository.GetVotePost(currentUserId, post.Id);
                            if (vote != null)
                            {
                                post.Upvote = vote.UpVote;
                                post.Downvote = vote.DownVote;
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
                foreach (var post in resultList)
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
                                    }

                                }
                            }
                        }

                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                        var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                        postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                foreach (var userDTO in UsersUpvote)
                                {
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                postDTO.Upvote = vote.UpVote;
                                postDTO.Downvote = vote.DownVote;
                            }
                        }
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
            var studentRole = _userRoleConstrant.GetStudentRole();
            var modRole = _userRoleConstrant.GetModeratorRole();
            if (exitedUser == null
                || exitedUser.Status == false
                || !(exitedUser.Role.Contains(studentRole) || exitedUser.Role.Contains(modRole))) return null;

            //return null if search Posts' list of userId is failed
            var existedPostList = _postRepository.SearchPostByUserId(userId);
            if (existedPostList == null || existedPostList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedPostList);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));

                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                var getImages = _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

                var postUpvote = _votePostRepository.GetAllUsersVotedBy(post.Id);

                var UsersUpvote = _mapper.Map<List<UserDTO>>(postUpvote);
                post.UsersUpvote = (UsersUpvote is not null && UsersUpvote.Count > 0) ? UsersUpvote : new List<UserDTO>();
                post.Upvotes = (UsersUpvote == null || UsersUpvote.Count == 0) ? 0 : UsersUpvote.Count;

                var postDownvote = _votePostRepository.GetAllUsersDownVotedBy(post.Id);

                var UsersDownvote = _mapper.Map<List<UserDTO>>(postDownvote);
                post.UsersDownvote = (UsersDownvote is not null && UsersDownvote.Count > 0) ? UsersDownvote : new List<UserDTO>();
                post.Downvotes = (UsersDownvote == null || UsersDownvote.Count == 0) ? 0 : UsersDownvote.Count;

                var validViewer = CheckCurrentUser(userId);
                if (validViewer != null)
                {
                    var vote = _votePostRepository.GetVotePost(userId, post.Id);
                    if (vote != null)
                    {
                        post.Upvote = vote.UpVote;
                        post.Downvote = vote.DownVote;
                    }
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
            var onlyStudentCategory = _categoryRepository.GetCategoryByName(_specialCategories.GetOnlyStudent());
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

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        if (!getCategories.Any(item => item.Id == onlyStudentCategory.Id))
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
                                        }

                                    }
                                }
                            }

                            postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                            postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                            var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                            postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                    foreach (var userDTO in UsersUpvote)
                                    {
                                        var user = _userRepository.GetUser(userDTO.Id);
                                        if (user != null)
                                        {
                                            if (user.Status)
                                            {
                                                var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                        var user = _userRepository.GetUser(userDTO.Id);
                                        if (user != null)
                                        {
                                            if (user.Status)
                                            {
                                                var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                    postDTO.Upvote = vote.UpVote;
                                    postDTO.Downvote = vote.DownVote;
                                }
                            }
                            resultList.Add(postDTO);
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
                                    }

                                }
                            }
                        }

                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                        var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                        postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                foreach (var userDTO in UsersUpvote)
                                {
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                postDTO.Upvote = vote.UpVote;
                                postDTO.Downvote = vote.DownVote;
                            }
                        }
                        resultList.Add(postDTO);
                    }

                }
            }
            //get related data for all post


            //return posts'list
            return resultList;
        }

        public PostDTO? UpdatePost(int postId, string title, string content,
                                                int[]? tagIds, int[]? categoryIds,
                                                string[]? videoURLs, string[]? imageURLs)
        {
            //check post is existed
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status == true) return null;

            //Update info of existed post
            existedPost.Title = title;
            existedPost.Content = content;
            existedPost.UpdatedAt = DateTime.Now;

            //Mapping existedPost to data type PostDTO which have more fields (videos, images, tags, categories)
            var updatingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (updatingPost is null) return null;

            //Get user who owns post
            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(updatingPost.Id));
            updatingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            //updating videos if it is successful, return null otherwise 
            if (videoURLs is not null && videoURLs.Length > 0)
            {
                var updatedVideos = UpdateVideosOfPost(postId, videoURLs);
                if (updatedVideos is null) return null;
                updatingPost.Videos = updatedVideos;
            }
            else updatingPost.Videos = new List<VideoDTO>();

            //Updating images if it is successful.Otherwise, return null
            if (imageURLs is not null && imageURLs.Length > 0)
            {
                var updatedImages = UpdateImagesOfPost(postId, imageURLs);
                if (updatedImages is null) return null;
                updatingPost.Images = updatedImages;
            }
            else updatingPost.Images = new List<ImageDTO>();

            if (tagIds is not null && tagIds.Length > 0)
            {
                //Disable all relationship of post and tags to update new
                var tagsOfPost = _postTagRepository.GetPostTagsByPostId(postId);
                foreach (var tag in tagsOfPost)
                {
                    var disabledTag = _tagHandlers.DisablePostTag(postId, tag.TagId);
                    if (disabledTag is null) return null;
                }

                //update tags for post if it is successful, return null otherwise
                var tags = AttachTagsForPost(updatingPost, tagIds);
                if (tags is null || tags.Count == 0) return null;
                updatingPost.Tags = tags;
            }
            else updatingPost.Tags = new List<TagDTO>();

            if (categoryIds is not null && categoryIds.Length > 0)
            {
                //Disable all relationship of post and categories to update new
                var categoriesOfPost = _postCategoryRepository.GetPostCategoriesByPostId(postId);
                foreach (var category in categoriesOfPost)
                {
                    var disabledCategory = _categoryHandlers.DisablePostCategory(postId, category.CategoryId);
                    if (disabledCategory is null) return null;
                }

                //update categories for post if it is successful, return null otherwise
                var categories = AttachCategoriesForPost(updatingPost, categoryIds);
                if (categories is null || categories.Count == 0) return null;
                updatingPost.Categories = categories;
            }
            else updatingPost.Categories = new List<CategoryDTO>();

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

        public ICollection<VideoDTO>? UpdateVideosOfPost(int postId, string[] videoURLs)
        {
            //disable videos of post if post has videos
            var videos = _videoHandlers.GetVideosByPost(postId);
            if (videos is not null && videos.Count > 0)
            {
                foreach (var video in videos)
                {
                    var successDelete = _videoHandlers.DisableVideo(video.Id);
                    if (successDelete is null) return null;
                }
            }

            //update videos
            var updatedVideos = _videoHandlers.CreateVideo(postId, videoURLs);
            if (updatedVideos is null || updatedVideos.Count == 0) return new List<VideoDTO>();
            else return updatedVideos;
        }

        public ICollection<ImageDTO>? UpdateImagesOfPost(int postId, string[] imageURLs)
        {
            // disable images of post if post has images
            var images = _imageHandlers.GetImagesByPost(postId);
            if (images is not null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var successDelete = _imageHandlers.DisableImage(image.Id);
                    if (successDelete is null) return null;
                }
            }

            //update images
            var updatedImages = _imageHandlers.CreateImage(postId, imageURLs);
            if (updatedImages is null || updatedImages.Count == 0) return new List<ImageDTO>();
            else return updatedImages;
        }

        public ICollection<PostDTO>? ViewPendingPostList()
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList();
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedList);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                var getImages = _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

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

        public ICollection<PostDTO>? ViewPendingPostListOf(int userId)
        {
            //return null if get pending posts' list is failed
            var existedList = _postRepository.ViewPendingPostList(userId);
            if (existedList == null || existedList.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(existedList);

            //get related data for all post
            foreach (var post in resultList)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(post.Id));
                post.User = (getUser is not null && getUser.Status) ? getUser : null;

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                var getImages = _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

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

                var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(post.Id));
                post.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(post.Id));
                post.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

                var getImages = _imageHandlers.GetImagesByPost(post.Id);
                post.Images = (getImages is not null && getImages.Count > 0) ? getImages : new List<ImageDTO>();

                var getVideos = _videoHandlers.GetVideosByPost(post.Id);
                post.Videos = (getVideos is not null && getVideos.Count > 0) ? getVideos : new List<VideoDTO>();

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

        public ICollection<PostDTO>? GetAllPosts(int[] categoryIDs, int[] tagIDs, string searchValue, int currentUserId)
        {
            //if both categories and tags are empty getallposts.
            if ((categoryIDs == null || categoryIDs.Length == 0) && (tagIDs == null || tagIDs.Length == 0) && (searchValue == null || searchValue.Equals(string.Empty)) && (currentUserId == null || currentUserId == 0))
            {
                return null;
            }
            if ((categoryIDs == null || categoryIDs.Length == 0) && (tagIDs == null || tagIDs.Length == 0) && (searchValue == null || searchValue.Equals(string.Empty)))
            {
                return GetAllPosts(currentUserId);
            }
            if ((categoryIDs == null || categoryIDs.Length == 0) && (tagIDs == null || tagIDs.Length == 0))
            {
                return SearchPostsByTitle(searchValue, currentUserId);
            }
            //get post list based on categoryiesIDs and tagIDs
            var postList = _postRepository.GetPost(categoryIDs, tagIDs);
            //check if null
            if (postList == null || postList.Count == 0)
            {
                return null;
            }
            //init new List
            var postListDTO = new List<PostDTO>();

            var onlyStudentCategory = _categoryRepository.GetCategoryByName(_specialCategories.GetOnlyStudent());
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

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        if (!getCategories.Any(item => item.Id == onlyStudentCategory.Id))
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
                                        }

                                    }
                                }
                            }

                            postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                            postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                            var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                            postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                    foreach (var userDTO in UsersUpvote)
                                    {
                                        var user = _userRepository.GetUser(userDTO.Id);
                                        if (user != null)
                                        {
                                            if (user.Status)
                                            {
                                                var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                        var user = _userRepository.GetUser(userDTO.Id);
                                        if (user != null)
                                        {
                                            if (user.Status)
                                            {
                                                var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                    postDTO.Upvote = vote.UpVote;
                                    postDTO.Downvote = vote.DownVote;
                                }
                            }
                            postListDTO.Add(postDTO);
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
                                    }

                                }
                            }
                        }

                        postDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

                        var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(postDTO.Id));
                        postDTO.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                        var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(postDTO.Id));
                        postDTO.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                foreach (var userDTO in UsersUpvote)
                                {
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                    var user = _userRepository.GetUser(userDTO.Id);
                                    if (user != null)
                                    {
                                        if (user.Status)
                                        {
                                            var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                postDTO.Upvote = vote.UpVote;
                                postDTO.Downvote = vote.DownVote;
                            }
                        }
                        postListDTO.Add(postDTO);
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

            //Mapping existedPost to data type PostDTO which have more fields (videos, images, tags, categories)
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
                                }

                            }
                        }
                    }

                    existingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

                    var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(existingPost.Id));
                    existingPost.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

                    var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(existingPost.Id));
                    existingPost.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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
                                if (user != null)
                                {
                                    if (user.Status)
                                    {
                                        var followRelationship = _followUserRepository.GetFollowRelationship(currentUser, user);
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
                                var user = _userRepository.GetUser(userDTO.Id);
                                if (user != null)
                                {
                                    if (user.Status)
                                    {
                                        var followRelationship = _followUserRepository.GetFollowRelationship(user, currentUser);
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
                            existingPost.Upvote = vote.UpVote;
                            existingPost.Downvote = vote.DownVote;
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

            //Mapping existedPost to data type PostDTO which have more fields (videos, images, tags, categories)
            var existingPost = _mapper.Map<PostDTO>(existedPost);
            //return null if mapping is failed
            if (existingPost is null) return null;

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByPostID(existingPost.Id));
            existingPost.User = (getUser is not null && getUser.Status) ? getUser : null;

            var getCategories = _mapper.Map<ICollection<CategoryDTO>?>(_postCategoryRepository.GetCategoriesOf(existingPost.Id));
            existingPost.Categories = (getCategories is not null && getCategories.Count > 0) ? getCategories : new List<CategoryDTO>();

            var getTags = _mapper.Map<ICollection<TagDTO>?>(_postTagRepository.GetTagsOf(existingPost.Id));
            existingPost.Tags = (getTags is not null && getTags.Count > 0) ? getTags : new List<TagDTO>();

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

            return existingPost;
        }

        public ICollection<PostDTO>? GetPostsHaveVideo(int currentUserId)
        {
            //get all posts
            var initPosts = GetAllPosts(currentUserId);
            if (initPosts is null || initPosts.Count == 0) return new List<PostDTO>();

            //instantiate return list
            var result = new List<PostDTO>();

            //add post to result list if post has at least 1 video
            foreach (var post in initPosts)
            {
                if (post.Videos is not null && post.Videos.Count > 0)
                {
                    result.Add(post);
                }
            }

            return result;
        }

        public ICollection<PostDTO>? GetPostsHaveImage(int currentUserId)
        {
            //get all posts
            var initPosts = GetAllPosts(currentUserId);
            if (initPosts is null || initPosts.Count == 0) return new List<PostDTO>();

            //instantiate return list
            var result = new List<PostDTO>();

            //add post to result list if post has at least 1 image
            foreach (var post in initPosts)
            {
                if (post.Images is not null && post.Images.Count > 0)
                {
                    result.Add(post);
                }
            }

            return result;
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
            var sortedCollection = postList.OrderByDescending(obj => obj.Upvotes).ToList();
            for (int i = sortedCollection.Count - 1; i >= 0; i--)
            {
                if (i < 5) break;
                sortedCollection.Remove(sortedCollection[i]);
            }
            if (sortedCollection.Count > 5)
            {
                sortedCollection = sortedCollection.GetRange(0, 5);
            }
            return sortedCollection;
        }
    }
}
