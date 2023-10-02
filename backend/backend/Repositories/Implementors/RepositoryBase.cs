using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace backend.Repositories.Implementors
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase()
        {
            _fBlogAcademyContext = new FBlogAcademyContext();
            _dbSet = _fBlogAcademyContext.Set<T>();
        }

        public bool Add(T entity)
        {
            _fBlogAcademyContext.Add(entity);
            return Save();
        }

        public ICollection<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetOne(T entity)
        {
            return _dbSet.Where(c => c.Equals(entity)).FirstOrDefault();
        }

        public bool IsExists(T entity)
        {
            return _dbSet.Any(c => c.Equals(entity));
        }

        public bool Remove(T entity)
        {
            _fBlogAcademyContext.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            var saved = _fBlogAcademyContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(T entity)
        {
            _fBlogAcademyContext.Update(entity);
            return Save();
        }
    }
}
