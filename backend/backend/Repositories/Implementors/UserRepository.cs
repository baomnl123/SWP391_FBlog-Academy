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
        public ICollection<User>? GetAllUsers()
        {
            try
            {
                var list = _fBlogAcademyContext.Users.OrderBy(u => u.Name).ToList();
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
        public ICollection<User>? GetAllDisableUser()
        {
            try
            {
                var userList = _fBlogAcademyContext.Users.Where(u => !u.Status).OrderBy(u => u.Name).ToList();
                if (userList == null || userList.Count == 0)
                {
                    return null;
                }
                return userList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public User? GetUser(string email)
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

        public User? GetUser(int id)
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
                var list = _fBlogAcademyContext.Users.Where(u => u.Name.Equals(username)).OrderBy(u => u.Name).ToList();
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
        public ICollection<User>? GetUsersByRole(string role)
        {
            try
            {
                var list = _fBlogAcademyContext.Users.Where(u => u.Role.Trim().Contains(role)).OrderBy(u => u.Name).ToList();
                if (list == null || list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (InvalidOperationException)
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
        public bool isExisted(int userID)
        {
            try
            {
                var user = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Id.Equals(userID));
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public bool isExisted(string email)
        {
            try
            {
                var checkUser = _fBlogAcademyContext.Users.FirstOrDefault(u => u.Email.Trim().Contains(email));
                if (checkUser == null)
                {
                    return false;
                }
                return true;
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
