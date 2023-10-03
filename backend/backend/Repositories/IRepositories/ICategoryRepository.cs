using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(string id);
        ICollection<Post> GetPostByCategory(string categoryId);
        bool CategoryExists(string id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
