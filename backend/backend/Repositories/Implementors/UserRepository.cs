using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class UserRepository : IUserRepository
    {
        private readonly FBlogAcademyContext _context;

        public UserRepository(FBlogAcademyContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public ICollection<Category> GetCategoryByAdmin(string adminId)
        {
            return _context.Categories.Where(c => c.AdminId.Equals(adminId) && c.Status.Equals(true)).ToList();
        }

        public ICollection<Tag> GetTagByAdmin(string adminId)
        {
            return _context.Tags.Where(c => c.AdminId.Equals(adminId) && c.Status.Equals(true)).ToList();
        }

        public User GetUser(string email, string password)
        {
            return _context.Users.Where(c => c.Email.Equals(email)
                                        && c.Password.Equals(password)
                                        && c.Status.Equals(true)).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.Where(c => c.Status.Equals(true)).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(c => c.Email.Trim().ToUpper() == email.Trim().ToUpper()
                                      && c.Status.Equals(true));
        }
    }
}
