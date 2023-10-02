namespace backend.Repositories.IRepositories
{
    public interface IRepositoryBase<T> where T : class
    {
        // GetById or GetByName
        public T GetOne(T entity);
        public ICollection<T> GetAll();
        public bool Add(T entity);
        public bool IsExists(T entity);
        public bool Remove(T entity);
        public bool Update(T entity);
        public bool Save();
    }
}
