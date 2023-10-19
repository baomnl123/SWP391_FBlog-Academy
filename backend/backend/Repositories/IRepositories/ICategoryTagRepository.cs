using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICategoryTagRepository
    {
        public bool CreateCategoryTag(CategoryTag categoryTag);
        public bool EnableCategoryTag(CategoryTag categoryTag);
        public bool DisableCategoryTag(CategoryTag categoryTag);
        public bool Save();
    }
}
