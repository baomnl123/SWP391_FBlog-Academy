using AutoMapper;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserRoleConstrant _userRoleConstrant;

        public CategoryHandlers(ICategoryRepository categoryRepository, IUserRepository userRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
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
                // If create succeed then return category, else return null
                if (_categoryRepository.CreateCategory(category)) 
                    return _mapper.Map<CategoryDTO>(category);
            }

            // If category was disabled, set status to true
            if (categoryExists.Status == false)
            {
                categoryExists.Status = true;
                // If enable succeed then return category, else return null
                if (_categoryRepository.EnableCategory(categoryExists)) 
                    return _mapper.Map<CategoryDTO>(categoryExists);
            }

            return null;
        }

        public CategoryDTO? UpdateCategory(string currentCategoryName, string newCategoryName)
        {
            // If category was disabled or not found, return false
            var category = _categoryRepository.GetCategoryByName(currentCategoryName);
            if (category == null || category.Status == false) return null;

            // If update succeed then return true, else return false
            category.CategoryName = newCategoryName;
            category.UpdatedAt = DateTime.Now;

            // If update succeed then return category, else return null
            if (_categoryRepository.UpdateCategory(category)) 
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }

        public CategoryDTO? EnableCategory(int categoryId)
        {
            // If category was enabled, return false
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null || category.Status == true) return null;

            // If enable succeed then return category, else return null
            if (_categoryRepository.EnableCategory(category))
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }

        public CategoryDTO? DisableCategory(int categoryId)
        {
            // If category was disabled, return false
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null || category.Status == false) return null;

            // If disable succeed then return category, else return null
            if (_categoryRepository.EnableCategory(category))
                return _mapper.Map<CategoryDTO>(category);

            return null;
        }
    }
}
