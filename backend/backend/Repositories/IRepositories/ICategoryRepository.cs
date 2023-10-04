using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        public ICollection<Category> GetCategories();
        public Category GetCategory(int categoryId);
        public ICollection<Post> GetPostByCategory(int categoryId);
        public bool CategoryExists(string name);
        public bool CreateCategory(Category category);
        public bool UpdateCategory(Category category);
        public bool DeleteCategory(Category category);
        public bool Save();
    }
}
