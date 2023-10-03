using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.IRepositories
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string id);
        ICollection<Category> GetCategoryByAdmin(string adminId);
        ICollection<Tag> GetTagByAdmin(string adminId);
        bool UserExists(string id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
