using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Handlers.Implementors
{
    public class CategoryHandlers : ICategoryHandlers
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CategoryHandlers(ICategoryRepository categoryRepository,
                                ICategoryTagRepository categoryTagRepository,
                                IPostCategoryRepository postCategoryRepository,
                                IUserRepository userRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _postCategoryRepository = postCategoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ICollection<CategoryDTO> GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public ICollection<CategoryDTO> GetDisableCategories()
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

        public bool CreateCategory(int adminId, string categoryName, int[] tagIds)
        {
            // Cannot find admin, return false
            var admin = _userRepository.GetUser(adminId);
            if (admin == null || admin.Role != "Admin") return false;

            var categoryExists = _categoryRepository.GetCategoryByName(categoryName);

            // If category does not have in DB
            if (categoryExists == null)
            {
                Category category = new()
                {
                    AdminId = adminId,
                    CategoryName = categoryName,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                foreach (var tagId in tagIds)
                {
                    _categoryRepository.CreateCategory(tagId, postId, category);
                }
                // If create succeed then return true, else return false
            }

            // If category was disabled, set status to true
            if (categoryExists.Status == false)
            {
                categoryExists.Status = true;
                _categoryRepository.EnableCategory(categoryExists);
                return true;
            }

            return false;
        }

        public bool UpdateCategory(string currentCategoryName, string newCategoryName)
        {
            // If category was disabled or not found, return false
            var category = _categoryRepository.GetCategoryByName(currentCategoryName);
            if (category == null || category.Status == false) return false;

            // If update succeed then return true, else return false
            category.CategoryName = newCategoryName;
            category.UpdatedAt = DateTime.Now;
            return _categoryRepository.UpdateCategory(category);
        }

        public bool EnableCategory(int categoryId)
        {
            // If category was enabled, return false
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category.Status == true) return true;

            // Enable CategoryTag
            var categoryTagList = _categoryTagRepository.GetCategoryTagByCategoryId(categoryId);
            if (categoryTagList != null)
            {
                foreach (var categoryTag in categoryTagList)
                {
                    _categoryTagRepository.EnableCategoryTag(categoryTag);
                }
            }

            // Enable PostCategory
            var postCategoryList = _postCategoryRepository.GetPostCategoryByCategoryId(categoryId);
            if (postCategoryList != null)
            {
                foreach (var postCategory in postCategoryList)
                {
                    _postCategoryRepository.EnablePostCategory(postCategory);
                }
            }

            // Return true if enable, false if cannot enable
            return _categoryRepository.EnableCategory(category);
        }

        public bool DisableCategory(int categoryId)
        {
            // If category was disabled, return false
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category.Status == false) return false;

            // Disable CategoryTag
            var categoryTagList = _categoryTagRepository.GetCategoryTagByCategoryId(categoryId);
            if (categoryTagList != null)
            {
                foreach (var categoryTag in categoryTagList)
                {
                    _categoryTagRepository.DisableCategoryTag(categoryTag);
                }
            }

            // Disable PostCategory
            var postCategoryList = _postCategoryRepository.GetPostCategoryByCategoryId(categoryId);
            if (postCategoryList != null)
            {
                foreach (var postCategory in postCategoryList)
                {
                    _postCategoryRepository.DisablePostCategory(postCategory);
                }
            }

            // Return true if disable, false if cannot disable
            return _categoryRepository.DisableCategory(category);
        }
    }
}
