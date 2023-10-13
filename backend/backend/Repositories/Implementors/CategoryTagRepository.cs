using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class CategoryTagRepository : ICategoryTagRepository
    {
        private readonly FBlogAcademyContext _context;

        public CategoryTagRepository()
        {
            _context = new();
        }

        public bool DisableCategoryTag(CategoryTag categoryTag)
        {
            categoryTag.Status = false;
            _context.Update(categoryTag);
            return Save();
        }

        public bool EnableCategoryTag(CategoryTag categoryTag)
        {
            categoryTag.Status = true;
            _context.Update(categoryTag);
            return Save();
        }

        public ICollection<CategoryTag> GetCategoryTagByCategoryId(int categoryId)
        {
            return _context.CategoryTags.Where(c => c.CategoryId == categoryId).ToList();
        }
        
        public ICollection<CategoryTag> GetCategoryTagByTagId(int tagId)
        {
            return _context.CategoryTags.Where(c => c.TagId == tagId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
