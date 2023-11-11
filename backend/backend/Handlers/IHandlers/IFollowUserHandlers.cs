using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IFollowUserHandlers
    {
        //Get Following | Followers
        public ICollection<UserDTO>? GetAllFollowerUsers(int currentUserID,int userID);
        public ICollection<UserDTO>? GetAllFollowingUsers(int currentUserID, int userID);
        //Follow Other Student
        public FollowUserDTO? FollowOtherUser(int currentUserID, int userID);
        public FollowUserDTO? UnfollowUser(int currentUserID, int userID);
    }
}
