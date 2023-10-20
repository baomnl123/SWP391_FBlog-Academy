using AutoMapper;
using Azure;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Handlers.Implementors
{
    public class CategoryHandlers : ICategoryHandlers
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;

        public CategoryHandlers(ITagRepository tagRepository,
                           ICategoryRepository categoryRepository,
                           ICategoryTagRepository categoryTagRepository,
                           IPostCategoryRepository postCategoryRepository,
                           IUserRepository userRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userRoleConstrant = new();
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
            if (category == null || category.Status == false) return null;

            return _mapper.Map<CategoryDTO>(category);
        }

        public CategoryDTO? GetCategoryByName(string categoryName)
        {
            var category = _categoryRepository.GetCategoryByName(categoryName);
            if (category == null || category.Status == false) return null;

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
            // Cannot find admin, return false
            var admin = _userRepository.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (admin == null || !admin.Role.Equals(adminRole)) return null;

            // Find category by name
            var categoryExists = _categoryRepository.GetCategoryByName(categoryName);
            // Create a new tag object if tagExists is null, or return tagExists otherwise.
            var category = categoryExists ?? new Category()
            {
                AdminId = adminId,
                CategoryName = categoryName,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            // If tag exists and is disabled, enable it and return it.
            if (categoryExists?.Status == false && EnableCategory(categoryExists.Id) != null)
                return _mapper.Map<CategoryDTO>(categoryExists);

            // Create the category, and return the mapped tag DTO if succeed.
            if (_categoryRepository.CreateCategory(category))
                return _mapper.Map<CategoryDTO>(category);

            // Otherwise, return null.
            return null;
        }

        public CategoryDTO? UpdateCategory(string currentCategoryName, string newCategoryName)
        {
            // Find category and categoryTag
            var category = _categoryRepository.GetCategoryByName(currentCategoryName);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByCategoryId(category.Id);
            if (category == null || category.Status == false || categoryTags == null) return null;

            // Set new CategoryName and UpdatedAt
            category.CategoryName = newCategoryName;
            category.UpdatedAt = DateTime.Now;

            // Return the mapped tag DTO if all updates succeeded, otherwise return null.
            return _categoryRepository.UpdateCategory(category) ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public CategoryDTO? EnableCategory(int categoryId)
        {
            // Find category and categoryTag
            var category = _categoryRepository.GetCategoryById(categoryId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByCategoryId(category.Id);
            var postCategories = _postCategoryRepository.GetPostCategoriesByCategoryId(category.Id);
            if (category == null || category.Status == true || categoryTags == null) return null;

            // Check if all enables succeeded.
            var check = categoryTags.All(categoryTag => _categoryTagRepository.EnableCategoryTag(categoryTag));
            postCategories.All(postCategory => _postCategoryRepository.EnablePostCategory(postCategory));

            // Return the mapped tag DTO if all enables succeeded, otherwise return null.
            return _categoryRepository.EnableCategory(category) && check ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public CategoryDTO? DisableCategory(int categoryId)
        {
            // Find category and categoryTag
            var category = _categoryRepository.GetCategoryById(categoryId);
            var categoryTags = _categoryTagRepository.GetCategoryTagsByCategoryId(category.Id);
            var postCategories = _postCategoryRepository.GetPostCategoriesByCategoryId(category.Id);
            if (category == null || category.Status == false || categoryTags == null) return null;

            // Check if all disable succeeded.
            var check = categoryTags.All(categoryTag => _categoryTagRepository.DisableCategoryTag(categoryTag));
            postCategories.All(postCategory => _postCategoryRepository.DisablePostCategory(postCategory));

            // Return the mapped tag DTO if all disable succeeded, otherwise return null.
            return _categoryRepository.DisableCategory(category) && check ? _mapper.Map<CategoryDTO>(category) : null;
        }
    }
}
