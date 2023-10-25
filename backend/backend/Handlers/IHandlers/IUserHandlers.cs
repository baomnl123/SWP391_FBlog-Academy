using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IUserHandlers
    {
        //Get User Account
        public ICollection<UserDTO>? GetAllUsers();
        public ICollection<UserDTO>? GetStudentsAndModerator();
        public ICollection<UserDTO>? GetLecturers();
        public ICollection<UserDTO>? GetAllDisableUsers(); 
        public UserDTO? GetUser(int userID);
        //Modify User
        public UserDTO? CreateUser(string name,string avatarURL, string email, string password);
        public UserDTO? CreateLecturer(string name, string avatarURL, string email, string password);
        public UserDTO? UpdateUser(int userID, string name, string avatarURL, string password);
        public UserDTO? DisableUser(int userID);
        //public UserDTO? UpdateLecturer(int userID);
        //public UserDTO? DisableLecturer(int userID);
        //Promote/Demote
        public UserDTO? PromoteStudent(int userID);
        public UserDTO? DemoteStudent(int userID);
        public UserDTO? GetUserByEmail(string email);
    }
}
