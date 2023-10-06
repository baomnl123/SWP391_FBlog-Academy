using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IFollowUserHandlers
    {
        //Get Following | Followers
        public ICollection<UserDTO>? GetAllFollowerUsers(int currentUserID);
        public ICollection<UserDTO>? GetAllFollowingUsers(int currentUserID);
        //Follow Other Student
        public FollowUserDTO? FollowOtherUser(int currentUserID, int userID);
        public bool UnfollowUser(int currentUserID, int userID);
    }
}
