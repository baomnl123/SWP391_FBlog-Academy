using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IFollowUserRepositoy
    {
        //Get Following | Follower
        public ICollection<User> GetAllFollowingUsers(User user);
        public ICollection<User> GetAllFollowerUsers(User user);
        public FollowUser GetFollowRelationship(User followUser, User followedUser);
        //Follow Other User
        public bool CreateFollowRelationShip(FollowUser followuser);
        public bool DisableFollowRelationship(FollowUser followuser);
        public bool UpdateFollowRelationship(FollowUser followuser);
        //Check Existed
        public bool isExisted(FollowUser followuser);

    }
}
