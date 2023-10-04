using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;

namespace backend.Handlers.Implementors
{
    public class UserHandlers : IUserHandlers
    {
        public UserDTO CheckLogin(string email, string password)
        {
            throw new NotImplementedException();
        }

        public UserDTO CreateLecturer()
        {
            throw new NotImplementedException();
        }

        public UserDTO CreateUser(string name, string username, string password)
        {
            throw new NotImplementedException();
        }

        public UserDTO DemoteStudent(int userID)
        {
            throw new NotImplementedException();
        }

        public UserDTO DisableLecturer(int userID)
        {
            throw new NotImplementedException();
        }

        public UserDTO DisableUser(int userID)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetFollowedUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetFollowingUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetLecturers()
        {
            throw new NotImplementedException();
        }

        public UserDTO LogOut()
        {
            throw new NotImplementedException();
        }

        public UserDTO PromoteStudent(int userID)
        {
            throw new NotImplementedException();
        }

        public UserDTO UpdateLecturer(int userID)
        {
            throw new NotImplementedException();
        }

        public UserDTO UpdateUser(int userID)
        {
            throw new NotImplementedException();
        }
    }
}
