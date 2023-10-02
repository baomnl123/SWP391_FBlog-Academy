namespace backend.Repositories.IRepositories
{
    // Group all CRUD and all Repositories which have the same function in one place
    public interface IRepositoryBase<T> where T : class
    {
        //GetOne func can GetById or GetByName
        public T GetOne(T entity);
        public ICollection<T> GetAll();
        public bool Add(T entity);
        public bool IsExists(T entity);
        public bool Remove(T entity);
        public bool Update(T entity);
        public bool Save();
    }
}
