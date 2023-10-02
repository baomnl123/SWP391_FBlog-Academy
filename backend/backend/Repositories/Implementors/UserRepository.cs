using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class UserRepository : IUserRepository
    {
        public ICollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetUsers(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetUsers(string username)
        {
            throw new NotImplementedException();
        }
    }
}
