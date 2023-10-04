using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;

namespace backend.Handlers.Implementors
{
    public class UserHandlers : IUserHandlers
    {
        public User CreateLecturer(string name, string email, string password)
        {
            throw new NotImplementedException();
        }

        public User CreateUser(string name, string email, string password)
        {
            throw new NotImplementedException();
        }

        public User DemoteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public bool DisableUser(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAllFollowedUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAllFollowingUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAllStudentAndModerators()
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetLecturers()
        {
            throw new NotImplementedException();
        }

        public bool IsTimeOut()
        {
            throw new NotImplementedException();
        }

        public User LogIn(string email)
        {
            throw new NotImplementedException();
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }

        public User PromoteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, string name, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
