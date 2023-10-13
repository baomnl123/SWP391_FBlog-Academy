using backend.Models;
using backend.Repositories.IRepositories;

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
            return _context.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
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
            var saved = _context.SaveChanges();
            // if saved > 0 then return true, else return false
            return saved > 0;
        }
    }
}
