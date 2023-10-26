using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostCategoryRepository()
        {
            _context = new();
        }

        public bool CreatePostCategory(PostCategory postCategory)
        {
            _context.Add(postCategory);
            return Save();
        }

        public bool DisablePostCategory(PostCategory postCategory)
        {
            postCategory.Status = false;
            _context.Update(postCategory);
            return Save();
        }

        public bool EnablePostCategory(PostCategory postCategory)
        {
            postCategory.Status = true;
            _context.Update(postCategory);
            return Save();
        }

        public bool UpdatePostCategory(PostCategory postCategory)
        {
            _context.Update(postCategory);
            return Save();
        }

        public PostCategory? GetPostCategory(int postId, int categoryId)
        {
            return _context.PostCategories.FirstOrDefault(c => c.PostId == postId && c.CategoryId == categoryId);
        }

        public ICollection<PostCategory> GetPostCategoriesByCategoryId(int categoryId)
        {
            return _context.PostCategories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public ICollection<PostCategory> GetPostCategoriesByPostId(int postId)
        {
            return _context.PostCategories.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<Category>? GetCategoriesOf(int postId)
        {
            List<Category> categories = new();
            var list = _context.PostCategories.Where(r => r.PostId == postId && r.Status).Select(r => r.Category).ToList();
            if(list == null || list.Count == 0) return null;
            foreach(var item in list)
            {
                if(item.Status) categories.Add(item);
            }
            return categories.Count == 0? null : categories;
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
