using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

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
        public FollowUserDTO? FollowOtherUser(int currentUserID, int userID)
        {
            //Get current User and followUser info
            var currentUser = _userRepository.GetUser(currentUserID);
            var followedUser = _userRepository.GetUser(userID);
            //Check if null
            if (currentUser == null || followedUser == null)
            {
                return null;
            }
            //Check if self-follow
            if (currentUser == followedUser)
            {
                return null;
            }
            //Get follow relationship of 2 users
            var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser, followedUser);
            //If follow relationship is exist
            if (followRelationship != null)
            {
                //If it is available then return nothing
                if (followRelationship.Status == true)
                {
                    return null;
                }
                //If it is not available then reactivate
                followRelationship.Status = true;
                followRelationship.CreatedAt = DateTime.Now;
                //Update the relationship
                if (!_followUserRepositoy.UpdateFollowRelationship(followRelationship))
                {
                    return null;
                }
                //Return
                return _mapper.Map<FollowUserDTO>(followRelationship);
            }
            //If follow relationship is not exist then create new one
            FollowUser newRelationship = new()
            {
                FollowerId = currentUserID,
                FollowedId = userID,
                Status = true,
                CreatedAt = DateTime.Now,
            };
            //Add relationship
            if (!_followUserRepositoy.AddFollowRelationship(newRelationship))
            {
                return null;
            }
            //Return
            return _mapper.Map<FollowUserDTO>(newRelationship);
        }

        public ICollection<UserDTO>? GetAllFollowerUsers(int currentUserID)
        {
            //Init list
            List<UserDTO> listResult = new();
            //Get current user info
            var currentUser = _userRepository.GetUser(currentUserID);
            //if user is not avaiable
            if (currentUser == null || currentUser.Status == false)
            {
                return null;
            }
            //get list of its followers
            var list = _followUserRepositoy.GetAllFollowerUsers(currentUser);
            //return nothing if it is empty
            if (list == null || list.Count == 0)
            {
                return null;
            }
            //map to userdto
            foreach (var user in list)
            {
                //check status
                if (user.Status)
                {
                    //map to dto
                    listResult.Add(_mapper.Map<UserDTO>(user));
                }
            }
            //return list
            return listResult;
        }

        public ICollection<UserDTO>? GetAllFollowingUsers(int currentUserID)
        {
            List<UserDTO> listResult = new();
            //Get currentUserData
            var currentUser = _userRepository.GetUser(currentUserID);
            //Check null
            if (currentUser == null)
            {
                return null;
            }
            else
            {
                //Get FollowingRelationship
                var list = _followUserRepositoy.GetAllFollowingUsers(currentUser);
                //if list is empty return nothing
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
                    //return list
                    return listResult;
                }
            }
        }

        public FollowUserDTO? UnfollowUser(int currentUserID, int userID)
        {
            //Get Users Data
            var currentUser = _userRepository.GetUser(currentUserID);
            var followedUser = _userRepository.GetUser(userID);

            //if current user is unavailable then return nothing
            if (currentUser == null || currentUser.Status == false)
            {
                return null;
            }
            //if followed user is unavailable then return nothing
            if (followedUser == null || followedUser.Status == false)
            {
                return null;
            }
            //if user self-follow then return nothing
            if (currentUser == followedUser)
            {
                return null;
            }
            //Get Follow Relationship
            var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser, followedUser);
            //if null or already disabled then return nothing
            if (followRelationship == null || followRelationship.Status == false)
            {
                return null;
            }
            //Disable Follow Relationship
            followRelationship.Status = false;
            if (!_followUserRepositoy.UpdateFollowRelationship(followRelationship))
            {
                return null;
            }
            return _mapper.Map<FollowUserDTO>(followRelationship);
        }
    }
}

