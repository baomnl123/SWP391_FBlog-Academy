using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IUserHandlers
    {
        //Authentication
        public UserDTO CheckLogin();
        public UserDTO Register(UserDTO user);
        //Follow User
        public ICollection<User> GetFollowingUsers();
        public ICollection<User> GetFollowedUsers();
        //Lecturers' Account
        public ICollection<User> GetLecturers();
        public UserDTO CreateLecturer();
        public UserDTO UpdateLecturer();
        public UserDTO DeleteLecturer();
        //
    }
}
