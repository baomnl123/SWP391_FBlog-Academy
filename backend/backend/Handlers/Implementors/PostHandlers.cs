using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostHandlers : IPostHandlers
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoHandlers _videoHandlers;
        private readonly IImageHandlers _imageHandlers;
        private readonly ITagHandlers _tagHandlers;
        private readonly ICategoryHandlers _categoryHandlers;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;

        public PostHandlers(IPostRepository postRepository,
                            IUserRepository userRepository,
                            IVideoHandlers videoHandlers,
                            IImageHandlers imageHandlers,
                            ITagHandlers tagHandlers,
                            ICategoryHandlers categoryHandlers,
                            IPostTagRepository postTagRepository,
                            IPostCategoryRepository postCategoryRepository,
                            IMapper mapper)
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
        }

        public PostDTO? ApprovePost(int reviewerId, int postId)
        {
            //check reviewer is not null
            //                  and not yet removed
            //                  and has role MOD(Moderator) or LT(Lecturer)
            var reviewer = _userRepository.GetUser(reviewerId);
            if (reviewer != null
                && reviewer.Status == true
                && (reviewer.Role.Contains("MOD") || reviewer.Role.Contains("LT")))
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

                //Update info to database
                if (!_postRepository.UpdatePost(existedPost)) return null;
                return _mapper.Map<PostDTO>(existedPost);
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

            //create videos for post if it is necessary
            if (videoURLs is not null)
            {
                var videos = _videoHandlers.CreateVideo(createdPost.Id, videoURLs);
                if (videos is null)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Videos = videos;
            }
            else createdPost.Videos = null;

            //create images for post if it is necessary
            if (imageURLs is not null)
            {
                var images = _imageHandlers.CreateImage(createdPost.Id, imageURLs);
                if (images is null)
                {
                    Delete(createdPost.Id);
                    return null;
                };
                createdPost.Images = images;
            }
            else createdPost.Images = null;

            //add categories for post if it is necessary
            if (categoryIds is not null)
            {
                var categories = AttachCategoriesForPost(createdPost, categoryIds);
                if (categories is null) return null;
                createdPost.Categories = categories;
            }
            else createdPost.Categories = null;

            //add tags for post if it is successful, return null otherwise
            if (tagIds is not null)
            {
                var tags = AttachTagsForPost(createdPost, tagIds);
                if (tags is null) return null;
                createdPost.Tags = tags;
            }
            else createdPost.Tags = null;

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
                if (category is null)
                {
                    Delete(createdPost.Id);
                    return null;
                };

                //Undo creating post and return null if relationship of post and category does not exist or is removed
                var addedCategory = _categoryHandlers.CreatePostCategory(createdPost, category);
                if (addedCategory is null)
                {
                    Delete(createdPost.Id);
                    return null;
                }

                //add category to categories' list if creating reltionship is successful
                categories.Add(addedCategory);
            }

            return categories;
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

            //return null if disabling all data related to post is failed
            var successDisabled = DisableAllRelatedToPost(deletingPost);
            if (successDisabled == null) return null;

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
            deletingPost.Videos = null;

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
            deletingPost.Images = null;

            //disable tags of post if post has tags
            var tagsOfPost = _postTagRepository.GetPostTagsByPostId(deletingPost.Id);
            foreach (var tag in tagsOfPost)
            {
                var disabledTag = _tagHandlers.DisablePostTag(deletingPost.Id, tag.TagId);
                if (disabledTag is null) return null;
            }
            deletingPost.Tags = null;

            //disable categories of post if post has categories
            var categoriesOfPost = _postCategoryRepository.GetPostCategoriesByPostId(deletingPost.Id);
            foreach (var category in categoriesOfPost)
            {
                var disabledCategory = _categoryHandlers.DisablePostCategory(deletingPost.Id, category.CategoryId);
                if (disabledCategory is null) return null;
            }
            deletingPost.Categories = null;

            return deletingPost;
        }
        public PostDTO? DenyPost(int reviewerId, int postId)
        {
            //return null if validReviewer is null
            //                              or removed
            //                              or does not have role MOD(Moderator) or LT(Lecturer)
            var validReviewer = _userRepository.GetUser(reviewerId);
            if (validReviewer == null
                || validReviewer.Status == false
                || !(validReviewer.Role.Contains("MD") || validReviewer.Role.Contains("LT"))) return null;

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

            //return null if disabling all data related to post is failed
            var successDisabled = DisableAllRelatedToPost(disablingPost);
            if (successDisabled == null) return null;

            //update info to database
            if (!_postRepository.UpdatePost(existedPost)) return null;
            return _mapper.Map<PostDTO>(existedPost);
        }

        public ICollection<PostDTO>? GetAllPosts()
        {
            var existed = _postRepository.GetAllPosts();
            return _mapper.Map<List<PostDTO>>(existed);
        }

        public ICollection<PostDTO>? SearchPostByUserId(int userId)
        {
            //check that user is not null
            //                  or removed
            //                  or that user does not have role SU(Student) or role MOD(Moderator)
            var exitedUser = _userRepository.GetUser(userId);
            if (exitedUser == null
                || exitedUser.Status == false
                || !(exitedUser.Role.Contains("SU") || exitedUser.Role.Contains("MOD"))) return null;

            //Search Posts' list of userId
            var existedPostList = _postRepository.SearchPostByUserId(userId);

            //check null
            if (existedPostList == null) return null;

            //return posts' list
            return _mapper.Map<List<PostDTO>>(existedPostList);
        }

        public ICollection<PostDTO>? SearchPostsByTitle(string title)
        {
            //init listPost to return
            List<PostDTO> listPost = new List<PostDTO>();

            //Search all posts which contain content
            var list = _postRepository.SearchPostsByTitle(title);

            //check null
            if (list != null)
            {
                //Map to List<PostDTO>
                listPost = _mapper.Map<List<PostDTO>>(list);
                return listPost;
            }
            return null;
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

            //updating videos if it is successful, return null otherwise 
            if (videoURLs is not null && videoURLs.Length > 0)
            {
                var updatedVideos = UpdateVideosOfPost(postId, videoURLs);
                if (updatedVideos is null) return null;
                updatingPost.Videos = updatedVideos;
            }
            else updatingPost.Videos = null;

            //Updating images if it is successful.Otherwise, return null
            if (imageURLs is not null && imageURLs.Length > 0)
            {
                var updatedImages = UpdateImagesOfPost(postId, imageURLs);
                if (updatedImages is null) return null;
                updatingPost.Images = updatedImages;
            }
            else updatingPost.Images = null;

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
                if (tags is null) return null;
                updatingPost.Tags = tags;
            }
            else updatingPost.Tags = null;

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
                if (categories is null) return null;
                updatingPost.Categories = categories;
            }
            else updatingPost.Categories = null;

            //Update to database
            if (!_postRepository.UpdatePost(existedPost)) return null;
            return updatingPost;
        }

        public ICollection<VideoDTO>? UpdateVideosOfPost(int postId, string[] videoURLs)
        {
            //create videos' list to return
            List<VideoDTO> updatedVideos = new();

            //create videos if post doesn't have videos
            List<VideoDTO>? videosOfPost = (List<VideoDTO>?)_videoHandlers.GetVideosByPost(postId);
            if (videosOfPost is null)
            {
                var videos = _videoHandlers.CreateVideo(postId, videoURLs);
                if (videos is null) return null;
                else return videos;
            };

            //
            int indexOfURL = 0;
            foreach (var video in videosOfPost)
            {
                var updatedVideo = _videoHandlers.UpdateVideo(postId, video.Id, videoURLs[indexOfURL++]);
                if (updatedVideo != null)
                {
                    updatedVideos.Add(updatedVideo);
                }
                else return null;
            }
            return updatedVideos;
        }

        public ICollection<ImageDTO>? UpdateImagesOfPost(int postId, string[] imageURLs)
        {
            //Crate images' list to return
            List<ImageDTO> updatedImages = new();

            //Creating images of post if post does not have images
            List<ImageDTO>? imagesOfPost = (List<ImageDTO>?)_imageHandlers.GetImagesByPost(postId);
            if (imagesOfPost is null)
            {
                var images = _imageHandlers.CreateImage(postId, imageURLs);
                if (images is null) return null;
                else return images;
            };

            //Updating images' list if post has images
            int indexOfURL = 0;
            foreach (var image in imagesOfPost)
            {
                var updatedImage = _imageHandlers.UpdateImage(postId, image.Id, imageURLs[indexOfURL++]);
                if (updatedImage != null)
                {
                    updatedImages.Add(updatedImage);
                }
                else return null;
            }
            return updatedImages;
        }

        public ICollection<PostDTO>? ViewPendingPostList()
        {
            //Get pending posts list
            var existedList = _postRepository.ViewPendingPostList();

            //check null
            if (existedList == null) return null;

            //Map to List<PostDTO>
            return _mapper.Map<List<PostDTO>>(existedList);
        }
    }
}
