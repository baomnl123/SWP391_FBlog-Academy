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
            //check if null
            if (currentUser == null || !currentUser.Status)
            {
                return null;
            }
            var followedUser = _userRepository.GetUser(userID);
            //Check if null
            if (followedUser == null || !followedUser.Status)
            {
                return null;
            }
            //Check if self-follow
            if (currentUser.Equals(followedUser))
            {
                return null;
            }
            //Get follow relationship of 2 users
            var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser, followedUser);
            //If follow relationship is exist
            if (followRelationship != null)
            {
                //If it is available then return nothing
                if (followRelationship.Status)
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
            //Get current user info
            var currentUser = _userRepository.GetUser(currentUserID);
            //if user is not avaiable
            if (currentUser == null || !currentUser.Status)
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
            //Init list
            var listResult = new List<UserDTO>();
            //map to userdto
            foreach (var user in list)
            {
                //check status
                if (user.Status)
                {
                    //map to dto
                    var followRelationship = _followUserRepositoy.GetFollowRelationship(user,currentUser);
                    if (followRelationship != null && followRelationship.Status)
                    {
                        listResult.Add(_mapper.Map<UserDTO>(user));
                    }
                }
            }
            if(listResult.Count == 0)
            {
                return null;
            }
            //return list
            return listResult;
        }

        public ICollection<UserDTO>? GetAllFollowingUsers(int currentUserID)
        {
            //Get currentUserData
            var currentUser = _userRepository.GetUser(currentUserID);
            //Check null
            if (currentUser == null || !currentUser.Status)
            {
                return null;
            }
            //Get FollowingRelationship
            var list = _followUserRepositoy.GetAllFollowingUsers(currentUser);
            //if list is empty return nothing
            if (list == null || list.Count == 0)
            {
                return null;
            }
            //init new list
            var listResult = new List<UserDTO>();
            //Map To UserDTO
            foreach (var user in list)
            {
                if (user.Status)
                {
                    var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser,user);
                    if(followRelationship != null && followRelationship.Status)
                    {
                        listResult.Add(_mapper.Map<UserDTO>(user));
                    }
                }
            }
            if(listResult.Count == 0)
            {
                return null;
            }
            //return list
            return listResult;
        }

        public FollowUserDTO? UnfollowUser(int currentUserID, int userID)
        {
            //Get Users Data
            var currentUser = _userRepository.GetUser(currentUserID);
            //if current user is unavailable then return nothing
            if (currentUser == null || !currentUser.Status)
            {
                return null;
            }
            var followedUser = _userRepository.GetUser(userID);
            //if followed user is unavailable then return nothing
            if (followedUser == null || !followedUser.Status)
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
            if (followRelationship == null || !followRelationship.Status)
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

