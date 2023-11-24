using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IUserMajorHandlers
    {
        public ICollection<UserMajorDTO>? AddUserMajor(int currentUserID, int[] majorIDs);
        public ICollection<UserMajorDTO>? DeleteUserMajor(int currentUserID, int[] majorIDs);
        public ICollection<MajorDTO>? GetMajorsOf(int userID);
        public ICollection<UserDTO>? GetUsersOf(int majorID);
    }
}
