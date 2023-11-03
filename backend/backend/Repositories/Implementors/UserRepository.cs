using backend.DTO;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementors
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        public UserRepository()
        {
            this._userRoleConstrant = new();
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
                var list = _fBlogAcademyContext.Users.OrderBy(u => u.Name).OrderBy(u => u.Id).ToList();
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
                var userList = _fBlogAcademyContext.Users.Where(u => !u.Status).OrderBy(u => u.Id).ToList();
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
                var list = _fBlogAcademyContext.Users.Where(u => u.Name.Equals(username)).OrderBy(u => u.Id).ToList();
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
                var list = _fBlogAcademyContext.Users.Where(u => u.Role.Trim().Contains(role)).OrderBy(u => u.Id).ToList();
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

        public ICollection<User>? GetStudentsAndModerators()
        {
            try
            {
                var studentRole = _userRoleConstrant.GetStudentRole();
                var moderatorRole = _userRoleConstrant.GetModeratorRole();
                var list = _fBlogAcademyContext.Users.Where(e => e.Role.Equals(studentRole) || e.Role.Equals(moderatorRole)).OrderBy(e => e.Id).ToList();
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

        public User? GetUserByPostID(int postID)
        {
            try
            {
                var user = _fBlogAcademyContext.Posts.Where(e => e.Id.Equals(postID)).Select(e => e.User).FirstOrDefault();
                if (user == null || !user.Status)
                {
                    return null;
                }
                return user;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public User? GetUserByCommentID(int commentID)
        {
            try
            {
                var user = _fBlogAcademyContext.Comments.Where(e => e.Id.Equals(commentID)).Select(e => e.User).FirstOrDefault();
                if (user == null || !user.Status)
                {
                    return null;
                }
                return user;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
