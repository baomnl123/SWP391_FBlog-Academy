using backend.Models;
using backend.Repositories.IRepositories;
using System.Collections.Generic;

namespace backend.Repositories.Implementors
{
    public class UserRepository : IUserRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        public UserRepository()
        {
            this._fBlogAcademyContext = new();
        }
        public bool CreateUser(User user)
        {
            _fBlogAcademyContext.Add(user);
            if (_fBlogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }

        public bool DisableUser(User user)
        {
            user.Status = false;
            if (this.UpdateUser(user)) return true;
            return false;
        }

        public ICollection<User> GetAllUsers()
        {
            var list = _fBlogAcademyContext.Users.ToList();
            return list;
        }

        public User GetUserByEmail(string email)
        {
            var user = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Email.Equals(email));
            return user;
        }

        public User GetUserByID(int id)
        {
            var user = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Id.Equals(id));
            return user;
        }

        public ICollection<User> GetUsersByUsername(string username)
        {
            var list = _fBlogAcademyContext.Users.Where(u => u.Name.Equals(username)).ToList();
            return list;
        }

        public bool isExisted(User user)
        {
            var checkUser = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id));
            if (checkUser == null) return false;
            else return true;
        }

        public bool UpdateUser(User user)
        {
            _fBlogAcademyContext.Update(user);
            if (_fBlogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }
    }
}