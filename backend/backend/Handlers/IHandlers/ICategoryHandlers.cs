using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ICategoryHandlers
    {
        // Get list
        public ICollection<CategoryDTO>? GetCategories();
        public ICollection<CategoryDTO>? GetDisableCategories();
        // Get specific
        public CategoryDTO? GetCategoryById(int categoryId);
        public CategoryDTO? GetCategoryByName(string categoryName);
        public ICollection<PostDTO>? GetPostsByCategory(int categoryId);
        public ICollection<TagDTO>? GetTagsByCategory(int categoryId);
        public ICollection<CategoryDTO>? GetTop5Categories();
        // CRUD
        public CategoryDTO? CreateCategory(int adminId, string categoryName);
        public CategoryDTO? UpdateCategory(int currentCategoryId, string newCategoryName);
        public CategoryDTO? EnableCategory(int categoryId);
        public CategoryDTO? DisableCategory(int categoryId);
        public CategoryDTO? CreatePostCategory(PostDTO post, CategoryDTO category);
        public CategoryDTO? DisablePostCategory(int postId, int categoryId);
    }
}
