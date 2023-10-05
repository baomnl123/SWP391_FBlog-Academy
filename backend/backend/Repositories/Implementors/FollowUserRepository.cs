using backend.DTO;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Repositories.Implementors
{
    public class FollowUserRepository : IFollowUserRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly FBlogAcademyContext _fblogAcademyContext;

        public FollowUserRepository(IUserRepository userRepository)
        {
            _fblogAcademyContext = new();
            _userRepository = userRepository;
        }
        public bool AddFollowRelationship(FollowUser followuser)
        {
            try
            {
                _fblogAcademyContext.Add(followuser);
                if (_fblogAcademyContext.SaveChanges() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                } 
            }
            catch (InvalidOperationException)
            {
                return false;
            }
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
                if (!this.UpdateFollowRelationship(followuser))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public ICollection<User>? GetAllFollowerUsers(User user)
        {
            try
            {
                List<User> userList = new List<User>();
                var list = _fblogAcademyContext.FollowUsers.Where(u => u.FollowedId.Equals(user.Id)).ToList();
                if (list.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (var userInList in list)
                    {
                        var follower = _userRepository.GetUserByID(userInList.FollowerId);
                        if(follower != null) userList.Add(follower);
                    }
                    return userList;
                }
            }
            catch(InvalidOperationException)
            {
                return null;
            }
            
        }

        public ICollection<User>? GetAllFollowingUsers(User user)
        {
            try
            {
                List<User> userList = new List<User>();
                var list = _fblogAcademyContext.FollowUsers.Where(u => u.FollowerId.Equals(user.Id)).ToList();
                if (list.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (var userInList in list)
                    {
                        var following = _userRepository.GetUserByID(userInList.FollowedId);
                        if(following != null) userList.Add(following);
                    }
                    return userList;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public FollowUser? GetFollowRelationship(User followUser, User followedUser)
        {
            try
            {
                var relationship = _fblogAcademyContext.FollowUsers.FirstOrDefault(u => u.FollowedId.Equals(followedUser.Id)
                                                                                    && u.FollowerId.Equals(followUser.Id)
                                                                                    && u.Status == true);
                return relationship;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public bool isExisted(FollowUser followuser)
        {
            try
            {
                var checkFollowUser = _fblogAcademyContext.FollowUsers.FirstOrDefault(u => u.FollowedId.Equals(followuser.FollowedId)
                                                                                    && u.FollowerId.Equals(followuser.FollowerId));
                if (checkFollowUser == null)
                {
                    return false;
                }
                else return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
            
        }

        public bool UpdateFollowRelationship(FollowUser followuser)
        {
            try
            {
                _fblogAcademyContext.Update(followuser);
                if (_fblogAcademyContext.SaveChanges() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
