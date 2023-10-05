using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace backend.Handlers.Implementors
{
    public class FollowUserHandlers : IFollowUserHandlers
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ISession _session;
        private readonly IFollowUserRepositoy _followUserRepositoy;
        private readonly IUserRepository _userRepository;
        public FollowUserHandlers() {
            _followUserRepositoy = _serviceProvider.GetRequiredService<IFollowUserRepositoy>();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
        }
        public bool FollowUser(int userID)
        {
            var currentUserID32 = _session.GetInt32("userID");

            if (currentUserID32 == null)
            {
                return false;
            }
            else
            {
                var currentUserID = currentUserID32 ?? default;
                var currentUser = _userRepository.GetUserByID(currentUserID);
                var followedUser = _userRepository.GetUserByID(userID);

                var followRelationship = _followUserRepositoy.GetFollowRelationship(currentUser,followedUser);

                if(followRelationship != null)
                {
                    return false;
                }
                else
                {
                    
                }
            }
            return true;
        }

        public ICollection<UserDTO> GetAllFollowedUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetAllFollowingUsers()
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetFollowedUsersByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public ICollection<UserDTO> GetFollowingUsersByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public bool UnFollowUser(int userID)
        {
            throw new NotImplementedException();
        }

    }
}
