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
        private readonly IMapper _mapper;

        public PostHandlers(IPostRepository postRepository,
                            IUserRepository userRepository,
                            IVideoHandlers videoHandlers,
                            IImageHandlers imageHandlers,
                            ITagHandlers tagHandlers,
                            ICategoryHandlers categoryHandlers,
                            IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _videoHandlers = videoHandlers;
            _imageHandlers = imageHandlers;
            _tagHandlers = tagHandlers;
            _categoryHandlers = categoryHandlers;
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
                                                    int[] tagIds, int[] categoryIds,
                                                    string[] videoURLs, string[] imageURLs)
        {
            //return null if creating post is failed
            var createdPost = CreatePost(userId, title, content);
            if (createdPost == null) return null;

            //create videos for post
            var videos = _videoHandlers.CreateVideo(createdPost.Id, videoURLs);
            if (videos is null)
            {
                UndoCreate(createdPost.Id);
                return null;
            };
            createdPost.Videos = videos;

            //create images for post
            var images = _imageHandlers.CreateImage(createdPost.Id, imageURLs);
            if (images is null)
            {
                UndoCreate(createdPost.Id);
                return null;
            };
            createdPost.Images = images;

            //add tags for post if it is successful, return null otherwise
            var tags = AttachTagsForPost(createdPost, tagIds);
            if (tags is null) return null;
            createdPost.Tags = tags;


            //add categories for post
            var categories = AttachCategoriesForPost(createdPost, categoryIds);
            if (categories is null) return null;
            createdPost.Categories = categories;

            return createdPost;
        }

        public ICollection<TagDTO>? AttachTagsForPost(PostDTO createdPost, int[] tagIds)
        {
            List<TagDTO> tags = new();
            foreach (var tagId in tagIds)
            {
                var tag = _tagHandlers.GetTagById(tagId);

                if (tag is null)
                {
                    UndoCreate(createdPost.Id);
                    return null;
                };

                var addedTag = _tagHandlers.CreatePostTag(createdPost, tag);

                if (addedTag is null)
                {
                    UndoCreate(createdPost.Id);
                    return null;
                }

                tags.Add(addedTag);
            }

            return tags;
        }

        public ICollection<CategoryDTO>? AttachCategoriesForPost(PostDTO createdPost, int[] categoryIds)
        {
            List<CategoryDTO> categories = new();
            foreach (var categoryId in categoryIds)
            {
                var category = _categoryHandlers.GetCategoryById(categoryId);

                if (category is null)
                {
                    UndoCreate(createdPost.Id);
                    return null;
                };

                var addedCategory = _categoryHandlers.CreatePostCategory(createdPost, category);

                if (addedCategory is null)
                {
                    UndoCreate(createdPost.Id);
                    return null;
                }

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

        public PostDTO? UndoCreate(int postId)
        {
            var deletedPost = _postRepository.GetPost(postId);
            if (deletedPost == null
                || deletedPost.Status == false
                || deletedPost.IsApproved == false) return null;
            return _mapper.Map<PostDTO>(deletedPost);
        }

        public PostDTO? DenyPost(int reviewerId, int postId, int tagId, int categoryId)
        {
            //return null if validReviewer is null
            //                              or removed
            //                              or does not have role MOD(Moderator) or LT(Lecturer)
            var validReviewer = _userRepository.GetUser(reviewerId);
            if (validReviewer == null
                || validReviewer.Status == false
                || !(validReviewer.Role.Contains("MOD") || validReviewer.Role.Contains("LT"))) return null;

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
                                                int[] tagIds, int[] categoryIds,
                                                string[] videoURLs, string[] imageURLs)
        {
            //check post is existed
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !(existedPost.Status == true && existedPost.IsApproved == true)) return null;

            //Update info of existed post
            existedPost.Title = title;
            existedPost.Content = content;
            existedPost.UpdatedAt = DateTime.Now;

            //Update to database
            if (!_postRepository.UpdatePost(existedPost)) return null;
            return _mapper.Map<PostDTO>(existedPost);
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
