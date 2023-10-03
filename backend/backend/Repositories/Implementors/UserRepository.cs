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

        public ICollection<Category> GetCategoryByAdmin(int adminId)
        {
            return (ICollection<Category>)_context.Categories.Where(e => e.AdminId.Equals(adminId))
                                             .Select(c => c.CategoryName).ToList();
        }

        public ICollection<Tag> GetTagByAdmin(string adminId)
        {
            return (ICollection<Tag>)_context.Tags.Where(e => e.AdminId.Equals(adminId))
                                             .Select(c => c.TagName).ToList();
        }

        public User GetUser(string id)
        {
            return _context.Users.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
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

        public bool UserExists(string id)
        {
            return _context.Users.Any(c => c.Id.Equals(id));
        }
    }
}
