using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        // Get list
        public ICollection<Category> GetAllCategories();
        public ICollection<Category> GetDisableCategories();
        // Get specific
        public Category? GetCategoryById(int categoryId);
        public Category? GetCategoryByName(string categoryName);
        public ICollection<Post> GetPostsByCategory(int categoryId);
        public ICollection<Tag> GetTagsByCategory(int categoryId);
        // CRUD
        public bool CreateCategory(Category category);
        public bool UpdateCategory(Category category);
        public bool EnableCategory(Category category);
        public bool DisableCategory(Category category);
        public bool Save();
    }
}
