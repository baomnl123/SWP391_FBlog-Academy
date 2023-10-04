using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IUserRepository
    {
<<<<<<< HEAD
        //Get Users
        public ICollection<User> GetAllUsers();
        public User GetUserByID(int id);
        public User GetUserByEmail(string email);
        public ICollection<User> GetUsersByUsername(string username);
        //CRUD Users
        public bool CreateUser(User user);
        public bool UpdateUser(User user);
        public bool DisableUser(User user);
        //Check Exists
        public bool isExisted(User user);
=======
        ICollection<User> GetUsers();
        User GetUser(string email, string password);
        ICollection<Category> GetCategoryByAdmin(string adminId);
        ICollection<Tag> GetTagByAdmin(string adminId);
        bool UserExists(string email);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
>>>>>>> main
    }
}
