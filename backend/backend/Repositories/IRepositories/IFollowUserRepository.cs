using backend.DTO;
using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IFollowUserRepository
    {
        //Get Following | Follower
        public ICollection<User>? GetAllFollowingUsers(User user);
        public ICollection<User>? GetAllFollowerUsers(User user);
        public FollowUser? GetFollowRelationship(User followUser, User followedUser);
        //Follow Other User
        public bool AddFollowRelationship(FollowUser followuser);
        public bool DisableFollowRelationship(FollowUser followuser);
        public bool UpdateFollowRelationship(FollowUser followuser);
        //Check Existed
        public bool isExisted(FollowUser followuser);

    }
}
