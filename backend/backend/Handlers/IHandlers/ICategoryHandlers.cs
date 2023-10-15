using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ICategoryHandlers
    {
        // Get list
        public ICollection<CategoryDTO> GetCategories();
        public ICollection<CategoryDTO> GetDisableCategories();
        // Get specific
        public CategoryDTO? GetCategoryById(int categoryId);
        public CategoryDTO? GetCategoryByName(string categoryName);
        public ICollection<PostDTO>? GetPostsByCategory(int categoryId);
        public ICollection<TagDTO>? GetTagsByCategory(int categoryId);
        // CRUD
        public bool CreateCategory(int adminId, string categoryName, int[] tagIds);
        public bool UpdateCategory(string currentCategoryName, string newCategoryName);
        public bool EnableCategory(int categoryId);
        public bool DisableCategory(int categoryId);
    }
}
