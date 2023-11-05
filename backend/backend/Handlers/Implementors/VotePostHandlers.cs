using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class VotePostHandlers : IVotePostHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public VotePostHandlers(IUserRepository userRepository, IPostRepository postRepository, IVotePostRepository votePostRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
        }
        public VotePostDTO? CreateNewVotePost(int currentUserId, int postId, bool vote)
        {
            //return null if currentUser and post do not exist
            //                                          or are removed
            //                                          or post is not approved
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedPost = _postRepository.GetPost(postId);
            if (existedUser == null || !existedUser.Status
                || existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //update info of vote if that vote existed
            var existedVote = _votePostRepository.GetVotePost(currentUserId, postId);
            if (existedVote != null)
            {
                //return null if Upvote or Downvote is set
                if (existedVote.UpVote || existedVote.DownVote) return null;

                if (vote)
                {
                    existedVote.UpVote = true;
                    existedVote.DownVote = false;
                    existedVote.CreatedAt = DateTime.Now;
                }
                else
                {
                    existedVote.UpVote = false;
                    existedVote.DownVote = true;
                    existedVote.CreatedAt = DateTime.Now;
                };
                if (!_votePostRepository.Update(existedVote)) return null;
                return _mapper.Map<VotePostDTO>(existedVote);
            }

            //Create new vote
            VotePost newVote = new()
            {
                UserId = currentUserId,
                PostId = postId,
                CreatedAt = DateTime.Now,
            };
            if (vote)
            {
                newVote.UpVote = true;
                newVote.DownVote = false;
                newVote.CreatedAt = DateTime.Now;
            }
            else
            {
                newVote.UpVote = false;
                newVote.DownVote = true;
                newVote.CreatedAt = DateTime.Now;
            };

            //Add new vote to database
            if (!_votePostRepository.Add(newVote)) return null;
            return _mapper.Map<VotePostDTO>(newVote);
        }

        public VotePostDTO? DisableVotePost(int currentUserId, int postId)
        {
            //return null if currentUser and post do not exist
            //                                          or are removed
            //                                          or post is not approved
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedPost = _postRepository.GetPost(postId);
            if (existedUser == null || !existedUser.Status
                || existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if that vote does not exist or is disabled
            var existedVote = _votePostRepository.GetVotePost(currentUserId, postId);
            if (existedVote == null || (!existedVote.UpVote && !existedVote.DownVote)) return null;

            //set vote to be disabled
            existedVote.UpVote = false;
            existedVote.DownVote = false;

            //update to database
            if (!_votePostRepository.Update(existedVote)) return null;
            return _mapper.Map<VotePostDTO>(existedVote);
        }

        public ICollection<UserDTO>? GetAllUsersVotedBy(int postId)
        {
            //return null if post does not exist or is removed or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if user's list is empty
            var existedUsers = _votePostRepository.GetAllUsersVotedBy(postId);
            if (existedUsers == null || existedUsers.Count == 0) return null;

            //return user's list
            return _mapper.Map<List<UserDTO>>(existedUsers);
        }

        public ICollection<UserDTO>? GetAllUsersDownVotedBy(int postId)
        {
            //return null if post does not exist or is removed or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if user's list is empty
            var existedUsers = _votePostRepository.GetAllUsersDownVotedBy(postId);
            if (existedUsers == null || existedUsers.Count == 0) return null;

            //return user's list
            return _mapper.Map<List<UserDTO>>(existedUsers);
        }

        public VotePostDTO? UpdateVotePost(int currentUserId, int postId, bool vote)
        {
            //return null if currentUser and post do not exist
            //                                          or are removed
            //                                          or post is not approved
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedPost = _postRepository.GetPost(postId);
            if (existedUser == null || !existedUser.Status
                || existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if that vote does not exist
            var existedVote = _votePostRepository.GetVotePost(currentUserId, postId);
            if (existedVote == null) return null;

            //Update info of existed vote
            if (vote)
            {
                existedVote.UpVote = true;
                existedVote.DownVote = false;
            }
            else
            {
                existedVote.UpVote = false;
                existedVote.DownVote = true;
            };

            //return null if updating info to database is false
            if (!_votePostRepository.Update(existedVote)) return null;

            //return VotePost object if updating is successful
            return _mapper.Map<VotePostDTO>(existedVote);
        }
    }
}
