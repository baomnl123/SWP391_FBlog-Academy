using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementors
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostCategoryRepository()
        {
            _context = new();
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

        public ICollection<PostCategory> GetPostCategoryByCategoryId(int categoryId)
        {
            return _context.PostCategories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public ICollection<PostCategory> GetPostCategoryByPostId(int postId)
        {
            return _context.PostCategories.Where(c => c.PostId == postId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
