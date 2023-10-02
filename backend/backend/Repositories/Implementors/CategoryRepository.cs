using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementors
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;

        public CategoryRepository(FBlogAcademyContext fBlogAcademyContext)
        {
            _fBlogAcademyContext = fBlogAcademyContext;
        }
        public bool CategoryExists(int id)
        {
            return _fBlogAcademyContext.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _fBlogAcademyContext.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _fBlogAcademyContext.Remove(category);
            return Save();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _fBlogAcademyContext.Categories.ToList();
        }

        public Category GetCategoryById(string id)
        {
            return _fBlogAcademyContext.Categories.Where(e => e.Id.Equals(id)).FirstOrDefault();
        }

        public Category GetCategoryByName(string name)
        {
            return _fBlogAcademyContext.Categories.Where(e => e.Id.Equals(name)).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _fBlogAcademyContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
