using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class CategoryTagRepository : ICategoryTagRepository
    {
        private readonly FBlogAcademyContext _context;

        public CategoryTagRepository()
        {
            _context = new();
        }

        public bool CreateCategoryTag(CategoryTag categoryTag)
        {
            _context.Add(categoryTag);
            return Save();
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

        public bool UpdateCategoryTag(CategoryTag categoryTag)
        {
            _context.Update(categoryTag);
            return Save();
        }

        public CategoryTag? GetCategoryTag(int tagId, int categoryId)
        {
            return _context.CategoryTags.FirstOrDefault(c => c.TagId == tagId && c.CategoryId == categoryId);
        }

        public ICollection<CategoryTag> GetCategoryTagsByCategoryId(int categoryId)
        {
            return _context.CategoryTags.Where(c => c.CategoryId == categoryId).ToList();
        }

        public ICollection<CategoryTag> GetCategoryTagsByTagId(int tagId)
        {
            return _context.CategoryTags.Where(c => c.TagId == tagId).ToList();
        }

        public ICollection<Category>? GetCategoriesOf(int tagId)
        {
            List<Category> categories = new();
            var list = _context.CategoryTags.Where(c => c.TagId == tagId && c.Status).Select(c => c.Category).ToList();
            if (list is null || list.Count == 0) return null;
            foreach (var item in list)
            {
                if (item.Status) categories.Add(item);
            }
            return categories.Count == 0 ? null : categories;
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                // if saved > 0 then return true, else return false
                return saved > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}
