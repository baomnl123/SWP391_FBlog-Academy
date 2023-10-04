<<<<<<< HEAD
﻿using backend.Models;
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
=======
﻿using backend.Models;
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
>>>>>>> parent of 7c469ec (Merge branch 'main' of https://github.com/baomnl123/SWP391_FBlog-Academy)
