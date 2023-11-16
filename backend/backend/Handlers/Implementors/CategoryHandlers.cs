using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace backend.Handlers.Implementors
{
    public class CategoryHandlers : ICategoryHandlers
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVideoHandlers _videoHandlers;
        private readonly IImageHandlers _imageHandlers;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public CategoryHandlers(IUserRepository userRepository,
                                IVideoHandlers videoHandlers,
                                IImageHandlers imageHandlers,
                                ICategoryRepository categoryRepository,
                                ICategoryTagRepository categoryTagRepository,
                                IPostCategoryRepository postCategoryRepository,
                                IPostTagRepository postTagRepository,
                                IVotePostRepository votePostRepository,
                                IMapper mapper,
                                IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _videoHandlers = videoHandlers;
            _imageHandlers = imageHandlers;
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _postTagRepository = postTagRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public ICollection<CategoryDTO>? GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public ICollection<CategoryDTO>? GetDisableCategories()
        {
            var categories = _categoryRepository.GetDisableCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public CategoryDTO? GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null) return null;

            return _mapper.Map<CategoryDTO>(category);
        }

        public CategoryDTO? GetCategoryByName(string categoryName)
        {
            var category = _categoryRepository.GetCategoryByName(categoryName);
            if (category == null) return null;

            return _mapper.Map<CategoryDTO>(category);
        }

        public ICollection<PostDTO>? GetPostsByCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null || category.Status == false) return null;

            var posts = _categoryRepository.GetPostsByCategory(categoryId);
            if (posts == null || posts.Count == 0) return null;

            //map to list DTO
            List<PostDTO> resultList = _mapper.Map<List<PostDTO>>(posts);

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
                post.Upvotes = (postUpvote == null || postUpvote.Count == 0) ? 0 : postUpvote.Count;
            }

            //return posts'list
            return resultList;
        }

        public ICollection<TagDTO>? GetTagsByCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null || category.Status == false) return null;

            var tags = _categoryRepository.GetTagsByCategory(categoryId);
            if (tags == null || tags.Count == 0) return null;

            return _mapper.Map<List<TagDTO>>(tags);
        }

        public CategoryDTO? CreateCategory(int adminId, string categoryName)
        {
            // Create a new tacategory object 
            var category = new Category()
            {
                AdminId = adminId,
                CategoryName = categoryName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // Create the category, and return the mapped tag DTO if succeed.
            if (_categoryRepository.CreateCategory(category))
                return _mapper.Map<CategoryDTO>(category);

            // Otherwise, return null.
            return null;
        }

        public CategoryDTO? UpdateCategory(int currentCategoryId, string newCategoryName)
        {
            // Find category and categoryTag
            var category = _categoryRepository.GetCategoryById(currentCategoryId);
            if (category == null || category.Status == false) return null;

            // Set new CategoryName and UpdatedAt
            category.CategoryName = newCategoryName;
            category.UpdatedAt = DateTime.Now;

            // Return the mapped tag DTO if all updates succeeded, otherwise return null.
            return _categoryRepository.UpdateCategory(category) ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public CategoryDTO? EnableCategory(int categoryId)
        {
            // Find category and categoryTags and postCategories
            var category = _categoryRepository.GetCategoryById(categoryId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByCategoryId(category.Id);
            var postCategories = _postCategoryRepository.GetPostCategoriesByCategoryId(category.Id);
            if (category == null || category.Status == true) return null;

            // Check if all enables succeeded.
            var checkCategory = _categoryRepository.EnableCategory(category);
            var checkCategoryTag = categoryTags.All(categoryTag => _categoryTagRepository.EnableCategoryTag(categoryTag));
            var checkPostCategory = postCategories.All(postCategory => _postCategoryRepository.EnablePostCategory(postCategory));

            // Return the mapped tag DTO if all enables succeeded, otherwise return null.
            return (checkCategory) ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public CategoryDTO? DisableCategory(int categoryId)
        {
            // Find category and categoryTags and postCategories
            var category = _categoryRepository.GetCategoryById(categoryId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByCategoryId(category.Id);
            var postCategories = _postCategoryRepository.GetPostCategoriesByCategoryId(category.Id);
            if (category == null || category.Status == false) return null;
            // Check if all disable succeeded.
            var checkCategory = _categoryRepository.DisableCategory(category);
            var checkCategoryTag = categoryTags.All(categoryTag => _categoryTagRepository.DisableCategoryTag(categoryTag));
            var checkPostCategory = postCategories.All(postCategory => _postCategoryRepository.DisablePostCategory(postCategory));

            // Return the mapped tag DTO if all disable succeeded, otherwise return null.
            return (checkCategory) ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public CategoryDTO? CreatePostCategory(PostDTO post, CategoryDTO category)
        {
            // If relationship exists, then return null.
            var isExists = _postCategoryRepository.GetPostCategory(post.Id, category.Id);
            if (isExists != null && isExists.Status) return null;

            // If relationship is disabled, enable it and return it.
            if (isExists != null && !isExists.Status)
            {
                _postCategoryRepository.EnablePostCategory(isExists);
                return _mapper.Map<CategoryDTO>(category);
            }

            // Create a new postTag object if isExists is null, or return isExists otherwise.
            var postCategory = new PostCategory()
            {
                PostId = post.Id,
                CategoryId = category.Id,
                Status = true
            };

            // Add relationship
            if (_postCategoryRepository.CreatePostCategory(postCategory))
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }

        public CategoryDTO? DisablePostCategory(int postId, int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            var postCategory = _postCategoryRepository.GetPostCategory(postId, categoryId);
            if (postCategory == null) return null;
            // || postCategory.Status == false
            if (_postCategoryRepository.DisablePostCategory(postCategory))
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }

        public ICollection<CategoryDTO>? GetTop5Categories()
        {
            var categories = GetCategories().Where(c => c.Status).ToList();

            var topCategories = categories
              .Select(c => new
              {
                  Category = c,
                  VoteCount = _votePostRepository.GetAllUsersVotedBy(c.Id).Count
              })
              .OrderByDescending(x => x.VoteCount)
              .Take(5)
              .Select(x => x.Category)
              .ToList();

            return topCategories;
        }

    }
}
