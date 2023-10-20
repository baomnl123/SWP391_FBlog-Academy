using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryTagRepository
    {
        public CategoryTag GetCategoryTag(int tagId, int categoryId);
        public ICollection<CategoryTag> GetCategoryTagsByTagId(int tagId);
        public ICollection<CategoryTag> GetCategoryTagsByCategoryId(int categoryId);
        public bool CreateCategoryTag(CategoryTag categoryTag);
        public bool UpdateCategoryTag(CategoryTag categoryTag);
        public bool EnableCategoryTag(CategoryTag categoryTag);
        public bool DisableCategoryTag(CategoryTag categoryTag);
        public bool Save();
    }
}
