using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.IRepositories
{
    public interface IUserRepository
    {
        public ICollection<User> GetAllUsers();
        public ICollection<User> GetUsers(int id);
        public ICollection<User> GetUsers(string username);
    }
}
