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

        public bool CreateCategory(int tagId, int postId, Category category)
        {
            var tag = _context.Tags.FirstOrDefault(c => c.Id == tagId && c.Status == true);
            var post = _context.Posts.FirstOrDefault(c => c.Id == postId && c.Status == true);

            // Add CategoryTag
            var categoryTag = new CategoryTag()
            {
                Category = category,
                Tag = tag,
                Status = true
            };
            _context.Add(categoryTag);

            // Add PostCategory
            var postCategory = new PostCategory()
            {
                Post = post,
                Category = category,
                Status = true
            };
            _context.Add(postCategory);

            // Add Category
            _context.Add(category);
            return Save();
        }

        public bool DisableCategory(Category category)
        {
            category.Status = false;
            _context.Update(category);
            return Save();
        }

        public bool EnableCategory(Category category)
        {
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
            return _context.Categories.FirstOrDefault(e => e.Id == categoryId);
        }

        public Category? GetCategoryByName(string categoryName)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryName.Trim().ToUpper() == categoryName.Trim().ToUpper());
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

        public bool UpdateCategory(int tagId, int postId, Category category)
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
