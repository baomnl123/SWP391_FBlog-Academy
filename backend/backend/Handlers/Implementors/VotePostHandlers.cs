﻿using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class VotePostHandlers : IVotePostHandlers
    {
        private readonly IPostHandlers _postHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IVotePostRepository _votePostRepository;
        private readonly IMapper _mapper;

        public VotePostHandlers(IUserRepository userRepository, IPostRepository postRepository, IVotePostRepository votePostRepository, IMapper mapper, IUserHandlers userHandlers, IPostHandlers postHandlers)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _votePostRepository = votePostRepository;
            _mapper = mapper;
            _userHandlers = userHandlers;
            _postHandlers = postHandlers;
        }
        public VotePostDTO? CreateNewVotePost(int currentUserId, int postId, int vote)
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
                //Click the Upvote/Downvote AGAIN
                if (existedVote.Vote == vote)
                {
                    var votePostDTO = DisableVotePost(currentUserId, postId);
                    if (votePostDTO == null) return null;
                    return votePostDTO;
                }

                existedVote.Vote = vote;

                if (!_votePostRepository.Update(existedVote)) return null;

                var existedVoteDTO = _mapper.Map<VotePostDTO>(existedVote);
                existedVoteDTO.User = _mapper.Map<UserDTO>(existedUser);
                existedVoteDTO.Post = _mapper.Map<PostDTO>(_postHandlers.GetPostBy(existedPost.Id, currentUserId));

                return existedVoteDTO;
            }

            //Create new vote
            VotePost newVote = new()
            {
                UserId = currentUserId,
                PostId = postId,
                Vote = vote,
                CreatedAt = DateTime.Now,
            };


            //Add new vote to database
            if (!_votePostRepository.Add(newVote)) return null;

            var newVoteDTO = _mapper.Map<VotePostDTO>(newVote);

            var newUserDTO = _userHandlers.GetUser(currentUserId);
            newVoteDTO.User = newUserDTO;

            var newPostDTO = _postHandlers.GetPostBy(postId, currentUserId);
            newVoteDTO.Post = newPostDTO;

            return newVoteDTO;
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
            if (existedVote == null || existedVote.Vote == 0) return null;

            //set vote to be disabled
            existedVote.Vote = 0;

            //update to database
            if (!_votePostRepository.Update(existedVote)) return null;

            var existedVoteDTO = _mapper.Map<VotePostDTO>(existedVote);

            var userDTO = _userHandlers.GetUser(currentUserId);
            existedVoteDTO.User = userDTO;

            var postDTO = _postHandlers.GetPostBy(postId, currentUserId);
            existedVoteDTO.Post = postDTO;

            return existedVoteDTO;
        }

        public ICollection<UserDTO>? GetAllUsersVotedBy(int postId)
        {
            List<User>? result = new List<User>();

            //return null if post does not exist or is removed or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if user's list is empty
            var existedUsers = _votePostRepository.GetAllUsersVotedBy(postId);
            if (existedUsers == null || existedUsers.Count == 0) return null;

            foreach (var user in existedUsers)
            {
                if (user.Status)
                {
                    result.Add(user);
                }
            }

            //return user's list
            return _mapper.Map<List<UserDTO>>(result);
        }

        public ICollection<UserDTO>? GetAllUsersDownVotedBy(int postId)
        {
            List<User>? result = new List<User>();

            //return null if post does not exist or is removed or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !existedPost.Status || !existedPost.IsApproved) return null;

            //return null if user's list is empty
            var existedUsers = _votePostRepository.GetAllUsersDownVotedBy(postId);
            if (existedUsers == null || existedUsers.Count == 0) return null;

            foreach (var user in existedUsers)
            {
                if (user.Status)
                {
                    result.Add(user);
                }
            }

            //return user's list
            return _mapper.Map<List<UserDTO>>(result);
        }

        public VotePostDTO? UpdateVotePost(int currentUserId, int postId, int vote)
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
            existedVote.Vote = vote;

            //return null if updating info to database is false
            if (!_votePostRepository.Update(existedVote)) return null;

            //return VotePost object if updating is successful

            var existedVoteDTO = _mapper.Map<VotePostDTO>(existedVote);

            var userDTO = _userHandlers.GetUser(currentUserId);
            existedVoteDTO.User = userDTO;

            var postDTO = _postHandlers.GetPostBy(postId, currentUserId);
            existedVoteDTO.Post = postDTO;

            return existedVoteDTO;
        }
    }
}
