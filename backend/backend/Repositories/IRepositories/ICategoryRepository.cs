using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        public ICollection<Category> GetAllCategories();
        public Category GetCategoryById(string id);
        public Category GetCategoryByName(string name);
        public bool CategoryExists(int id);
        public bool CreateCategory(Category category);
        public bool UpdateCategory(Category category);
        public bool DeleteCategory(Category category);
        public bool Save();
    }
}
