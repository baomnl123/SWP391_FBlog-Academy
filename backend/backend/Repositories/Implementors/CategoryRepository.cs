using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FBlogAcademyContext _context;

        public CategoryRepository()
        {
            _context = new();
        }

        public bool CreateCategory(Category category)
        {
            // Add Category
            _context.Add(category);
            return Save();
        }

        public bool DisableCategory(Category category)
        {
            var postCategories = _context.PostCategories.Where(c => c.CategoryId == category.Id).ToList();

            foreach (var postCategory in postCategories)
            {
                postCategory.Status = false;
                _context.Update(postCategory);
            }

            category.Status = false;
            _context.Update(category);
            return Save();
        }

        public bool EnableCategory(Category category)
        {
            var categoryTags = _context.CategoryTags.Where(c => c.CategoryId ==  category.Id).ToList();
            var postCategories = _context.PostCategories.Where(c => c.CategoryId == category.Id).ToList();

            foreach (var categoryTag in categoryTags)
            {
                categoryTag.Status = true;
                _context.Update(categoryTag);
            }

            foreach (var postCategory in postCategories)
            {
                postCategory.Status = true;
                _context.Update(postCategory);
            }

            category.Status = true;
            _context.Update(category);
            return Save();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.Where(c => c.Status == true).ToList();
        }

        public ICollection<Category> GetDisableCategories()
        {
            return _context.Categories.Where(c => c.Status == false).ToList();
        }

        public Category? GetCategoryById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId && c.Status == true);
        }

        public Category? GetCategoryByName(string categoryName)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryName.Trim().ToUpper() == categoryName.Trim().ToUpper()
                                                      && c.Status == true);
        }

        public ICollection<Post> GetPostsByCategory(int categoryId)
        {
            return _context.PostCategories.Where(e => e.CategoryId == categoryId && e.Status == true)
                                          .Select(e => e.Post)
                                          .Where(c => c.Status == true).ToList();
        }

        public ICollection<Tag> GetTagsByCategory(int categoryId)
        {
            return _context.CategoryTags.Where(e => e.CategoryId == categoryId && e.Status == true)
                                        .Select(e => e.Tag)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
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
