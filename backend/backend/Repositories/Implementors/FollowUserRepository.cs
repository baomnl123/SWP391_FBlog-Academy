using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class FollowUserRepository : IFollowUserRepositoy
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly FBlogAcademyContext _fblogAcademyContext;

        public FollowUserRepository()
        {
            _fblogAcademyContext = new();
        }
        public bool CreateFollowRelationShip(FollowUser followuser)
        {
            _fblogAcademyContext.Add(followuser);
            if (_fblogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }

        public bool DisableFollowRelationship(FollowUser followuser)
        {
            if (!this.isExisted(followuser))
            {
                return false;
            }
            else
            {
                followuser.Status = false;
                if (this.UpdateFollowRelationship(followuser)) return true;
                else return false;
            }

        }

        public ICollection<User> GetAllFollowerUsers(User user)
        {
            var list = _fblogAcademyContext.FollowUsers.Where(u => u.FollowedId.Equals(user.Id)).ToList();
            List<User> userList = new List<User>();
            var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            foreach (var userInList in list)
            {
                userList.Add(userRepository.GetUserByID(userInList.FollowerId));
            }
            return userList;
        }

        public ICollection<User> GetAllFollowingUsers(User user)
        {
            var list = _fblogAcademyContext.FollowUsers.Where(u => u.FollowerId.Equals(user.Id)).ToList();
            List<User> userList = new List<User>();
            var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            foreach (var userInList in list)
            {
                userList.Add(userRepository.GetUserByID(userInList.FollowedId));
            }
            return userList;
        }

        public FollowUser GetFollowRelationship(User followUser, User followedUser)
        {
            var relationship = _fblogAcademyContext.FollowUsers.FirstOrDefault(u => u.FollowedId.Equals(followedUser.Id)
                                                                                    && u.FollowerId.Equals(followUser.Id));
            return relationship;
        }

        public bool isExisted(FollowUser followuser)
        {
            var checkFollowUser = _fblogAcademyContext.FollowUsers.FirstOrDefault(u => u.FollowedId.Equals(followuser.FollowedId)
                                                                                    && u.FollowerId.Equals(followuser.FollowerId));
            if (checkFollowUser == null)
            {
                return false;
            }
            else return true;
        }

        public bool UpdateFollowRelationship(FollowUser followuser)
        {
            _fblogAcademyContext.Update(followuser);
            if (_fblogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }
    }
}
