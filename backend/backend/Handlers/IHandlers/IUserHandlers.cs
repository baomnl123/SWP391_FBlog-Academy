using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IUserHandlers
    {
        //User Account
        public ICollection<User>? GetAllUsers();
        public ICollection<User>? GetStudentsAndModerator();
        public UserDTO? CreateUser(int userID, string name, string username,string password);
        public UserDTO? UpdateUser(int userID);
        public UserDTO? DisableUser(int userID);
        //Lecturers' Account
        public ICollection<UserDTO>? GetLecturers();
        public UserDTO? CreateLecturer(int userID, string name, string username, string password);
        public UserDTO? UpdateLecturer(int userID);
        public UserDTO? DisableLecturer(int userID);
        //Promote/Demote
        public UserDTO? PromoteStudent(int userID);
        public UserDTO? DemoteStudent(int userID);

    }
}
