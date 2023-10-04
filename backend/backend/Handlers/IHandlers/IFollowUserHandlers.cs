using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IFollowUserHandlers
    {
        //Get Following | Followers
        public ICollection<UserDTO> GetAllFollowingUsers();
        public ICollection<UserDTO> GetAllFollowedUsers();
        //Follow Other Student
        public bool FollowUser(int userID);
        public bool UnFollowUser(int userID);
    }
}
