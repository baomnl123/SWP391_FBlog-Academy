using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IFollowUserHandlers
    {
        //Get Following | Followers
        public ICollection<User> GetAllFollowingUsers();
        public ICollection<User> GetAllFollowedUsers();
        public ICollection<User> GetFollowingUsersByUsername(string username);
        public ICollection<User> GetFollowedUsersByUsername(string username);
        //Follow Other Student
        public bool FollowUser(User user);
        public bool UnFollowUser(User user);
    }
}
