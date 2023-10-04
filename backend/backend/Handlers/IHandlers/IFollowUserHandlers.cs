using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IFollowUserHandlers
    {
        //Get Following | Followers
        public ICollection<User> GetAllFollowingUsers();
        public ICollection<User> GetAllFollowedUsers();
        //Follow Other Student
        public bool FollowUser(int userID);
        public bool UnFollowUser(int userID);
    }
}
