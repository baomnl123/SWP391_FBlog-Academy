using backend.Models;
using backend.Repositories.IRepositories;

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
            if (_fBlogAcademyContext.SaveChanges() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool DisableUser(User user)
        {
            user.Status = false;
            if (!this.UpdateUser(user))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICollection<User>? GetAllUsers()
        {
            try
            {
                var list = _fBlogAcademyContext.Users.ToList();
                if(list == null || list.Count == 0)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public User? GetUserByEmail(string email)
        {
            try
            {
                var user = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Email.Equals(email));
                return user;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public User? GetUserByID(int id)
        {
            try
            {
                var user = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Id.Equals(id));
                return user;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<User>? GetUsersByUsername(string username)
        {
            try
            {
                var list = _fBlogAcademyContext.Users.Where(u => u.Name.Equals(username)).ToList();
                if (list == null || list.Count == 0)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public bool isExisted(User user)
        {
            try
            {
                var checkUser = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id));

                if (checkUser == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                _fBlogAcademyContext.Update(user);
                if (_fBlogAcademyContext.SaveChanges() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
