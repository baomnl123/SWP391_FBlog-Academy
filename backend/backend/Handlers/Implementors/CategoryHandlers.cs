using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Handlers.Implementors
{
    public class CategoryHandlers : ICategoryHandlers
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IMapper _mapper;

        public CategoryHandlers(ICategoryRepository categoryRepository,
                           ICategoryTagRepository categoryTagRepository,
                           IPostCategoryRepository postCategoryRepository,
                           IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _mapper = mapper;
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
            return _mapper.Map<List<PostDTO>>(posts);
        }

        public ICollection<TagDTO>? GetTagsByCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null || category.Status == false) return null;

            var tags = _categoryRepository.GetTagsByCategory(categoryId);
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
            if (postCategory == null || postCategory.Status == false) return null;

            if (_postCategoryRepository.DisablePostCategory(postCategory))
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }
    }
}
