using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.IRepositories
{
    public interface IUserRepository
    {
        //Get Users
        public ICollection<User> GetAllUsers();
        public User GetUserByID(string id);
        public User GetUserByEmail(string email);
        public ICollection<User> GetUsersByUsername(string username);
        //CRUD Users
        public bool CreateUser(User user);
        public bool UpdateUser(User user);
        public bool DisableUser(User user);
        //Check Exists
        public bool isExisted(User user);
    }
}
