using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace backend.Handlers.Implementors
{
    public class FollowUserHandlers : IFollowUserHandlers
    {
        private readonly IFollowUserRepository _followUserRepositoy;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public FollowUserHandlers(IMapper mapper, IFollowUserRepository followUserRepositoy, IUserRepository userRepository)
        {
            _followUserRepositoy = followUserRepositoy;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public bool FollowOtherUser(int currentUserID, int userID)
        {
            //Processing
            var currentUser = _userRepository.GetUserByID(currentUserID);
            var followedUser = _userRepository.GetUserByID(userID);

            if (currentUser == null || followedUser == null)
            {
                return false;
            }
            else
            {
                var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser, followedUser);

                if (followRelationship != null)
                {
                    return false;
                }
                else
                {
                    FollowUser newRelationship = new()
                    {
                        FollowerId = currentUserID,
                        FollowedId = userID,
                        Status = true,
                        CreatedAt = DateTime.Now,
                    };
                    if (!_followUserRepositoy.AddFollowRelationship(newRelationship)) return false;
                }
                return true;
            }
        }

        public ICollection<UserDTO>? GetAllFollowerUsers(int currentUserID)
        {
            List<UserDTO> listResult = new();
            var currentUser = _userRepository.GetUserByID(currentUserID);
            if (currentUser == null)
            {
                return null;
            }
            else
            {
                var list = _followUserRepositoy.GetAllFollowerUsers(currentUser);
                if (list == null)
                {
                    return null;
                }
                else
                {
                    foreach (var user in list)
                    {
                        listResult.Add(_mapper.Map<UserDTO>(user));
                    }
                    return listResult;
                }
            }
        }

        public ICollection<UserDTO>? GetAllFollowingUsers(int currentUserID)
        {
            List<UserDTO> listResult = new();
            //Get currentUserData
            var currentUser = _userRepository.GetUserByID(currentUserID);
            //Check null
            if (currentUser == null)
            {
                return null;
            }
            else
            {
                //Get FollowingRelationship
                var list = _followUserRepositoy.GetAllFollowingUsers(currentUser);

                if (list == null || list.Count == 0)
                {
                    return null;
                }
                else
                {
                    //Map To UserDTO
                    foreach (var user in list)
                    {
                        listResult.Add(_mapper.Map<UserDTO>(user));
                    }
                    return listResult;
                }
            }
        }

        public bool UnfollowUser(int currentUserID, int userID)
        {
            //Get Users Data
            var currentUser = _userRepository.GetUserByID(currentUserID);
            var followedUser = _userRepository.GetUserByID(userID);
            //Check null
            if (currentUser == null || followedUser == null)
            {
                return false;
            }
            else
            {
                //Get Follow Relationship
                var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser, followedUser);
                //Check null
                if (followRelationship == null)
                {
                    return false;
                }
                else
                {
                    //Disable Follow Relationship
                    followRelationship.Status = false;
                    if (!_followUserRepositoy.UpdateFollowRelationship(followRelationship)) return false;
                }
                return true;
            }
        }
    }
}
