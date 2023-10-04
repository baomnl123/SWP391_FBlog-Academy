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

        public bool CategoryExists(string name)
        {
            return _context.Categories.Any(c => c.CategoryName.Trim().ToUpper() == name.Trim().ToUpper() 
                                           && c.Status.Equals(true));
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
            return _context.Categories.Where(c => c.Status.Equals(true)).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(e => e.Id == id && e.Status.Equals(true)).FirstOrDefault();
        }

        public ICollection<Post> GetPostByCategory(int categoryId)
        {
            return _context.PostCategories.Where(e => e.PostId == categoryId)
                                          .Select(c => c.Post)
                                          .Where(c => c.Status.Equals(true)).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
