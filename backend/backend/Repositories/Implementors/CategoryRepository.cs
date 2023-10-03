using AutoMapper;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementors
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FBlogAcademyContext _context;

        public CategoryRepository(FBlogAcademyContext context)
        {
            _context = context;
        }

        public bool CategoryExists(string id)
        {
            return _context.Categories.Any(c => c.Id.Equals(id));
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(string id)
        {
            return _context.Categories.Where(e => e.Id.Equals(id)).FirstOrDefault();
        }

        public ICollection<Post> GetPostByCategory(string categoryId)
        {
            return _context.PostCategories.Where(e => e.PostId.Equals(categoryId))
                                          .Select(c => c.Post).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
