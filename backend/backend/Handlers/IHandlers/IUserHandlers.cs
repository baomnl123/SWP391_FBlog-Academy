using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IUserHandlers
    {
        //Authentication
        public UserDTO CheckLogin(string email,string password);
        public UserDTO LogOut();
        //Follow User
        public ICollection<UserDTO> GetFollowingUsers();
        public ICollection<UserDTO> GetFollowedUsers();
        //User Account
        public ICollection<User> GetAllUsers();
        public UserDTO CreateUser(string name, string username,string password);
        public UserDTO UpdateUser(int userID);
        public UserDTO DisableUser(int userID);
        //Lecturers' Account
        public ICollection<UserDTO> GetLecturers();
        public UserDTO CreateLecturer();
        public UserDTO UpdateLecturer(int userID);
        public UserDTO DisableLecturer(int userID);
        //Promote/Demote
        public UserDTO PromoteStudent(int userID);
        public UserDTO DemoteStudent(int userID);

    }
}
