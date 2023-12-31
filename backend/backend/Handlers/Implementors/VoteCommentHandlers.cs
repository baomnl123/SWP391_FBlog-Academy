﻿using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Handlers.Implementors
{
    public class VoteCommentHandlers : IVoteCommentHandlers
    {
        private readonly ICommentHandlers _commentHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly IVoteCommentRepository _voteCommentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public VoteCommentHandlers(IVoteCommentRepository voteCommentRepository,
                                    IUserRepository userRepository,
                                    ICommentRepository commentRepository,
                                    IMapper mapper,
                                    ICommentHandlers commentHandlers,
                                    IUserHandlers userHandlers)
        {
            _voteCommentRepository = voteCommentRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _commentHandlers = commentHandlers;
            _userHandlers = userHandlers;
        }

        public VoteCommentDTO? CreateVote(int currentUserId, int commentId, int vote)
        {
            //return null if currentUser and comment do not exist
            //                                          or are removed
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedUser == null || !existedUser.Status
                || existedComment == null || !existedComment.Status) return null;

            //check the existence of vote
            var existedVote = _voteCommentRepository.GetVoteComment(currentUserId, commentId);
            if (existedVote != null)
            {
                //return null if that vote is set
                if (existedVote.Vote != 0) return null;

                existedVote.Vote = vote;
                existedVote.CreateAt = DateTime.Now;

                if (!_voteCommentRepository.Update(existedVote)) return null;

                var existedVoteDTO = _mapper.Map<VoteCommentDTO>(existedVote);

                var userDTO = _userHandlers.GetUser(currentUserId);
                existedVoteDTO.User = userDTO;

                var commentDTO = _commentHandlers.GetCommentBy(commentId);
                existedVoteDTO.Comment = commentDTO;

                return existedVoteDTO;
            }

            //Create new vote
            VoteComment newVote = new()
            {
                UserId = currentUserId,
                CommentId = commentId,
                CreateAt = DateTime.Now,
            };

            newVote.Vote = vote;
            newVote.CreateAt = DateTime.Now;

            //Add new vote to database
            if (!_voteCommentRepository.Add(newVote)) return null;

            var newVoteDTO = _mapper.Map<VoteCommentDTO>(newVote);

            var newUserDTO = _userHandlers.GetUser(currentUserId);
            newVoteDTO.User = newUserDTO;

            var newCommentDTO = _commentHandlers.GetCommentBy(commentId);
            newVoteDTO.Comment = newCommentDTO;

            return newVoteDTO;
        }

        public ICollection<UserDTO>? GetAllUsersVotedBy(int commentId)
        {
            //return null if that comment does not exist
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedComment == null || !existedComment.Status) return null;

            //Get all users voting that comment
            var userList = _voteCommentRepository.GetAllUserBy(commentId);
            if (userList == null || userList.Count == 0) return null;
            return _mapper.Map<List<UserDTO>>(userList);
        }

        public VoteCommentDTO? UpdateVote(int currentUserId, int commentId, int vote)
        {
            //return null if currentUser and comment do not exist
            //                                          or are removed
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedUser == null || !existedUser.Status
                || existedComment == null || !existedComment.Status) return null;

            //check the existence of vote
            var existedVote = _voteCommentRepository.GetVoteComment(currentUserId, commentId);
            if (existedVote == null) return null;

            //set vote
            existedVote.Vote = vote;

            //update to databse
            if (!_voteCommentRepository.Update(existedVote)) return null;

            var existedVoteDTO = _mapper.Map<VoteCommentDTO>(existedVote);

            var userDTO = _userHandlers.GetUser(currentUserId);
            existedVoteDTO.User = userDTO;

            var commentDTO = _commentHandlers.GetCommentBy(commentId);
            existedVoteDTO.Comment = commentDTO;
            return existedVoteDTO;
        }

        public VoteCommentDTO? DisableVote(int currentUserId, int commentId)
        {
            //return null if currentUser and comment do not exist
            //                                          or are removed
            var existedUser = _userRepository.GetUser(currentUserId);
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedUser == null || !existedUser.Status
                || existedComment == null || !existedComment.Status) return null;

            //return null if vote does not exist or is disabled
            var existedVote = _voteCommentRepository.GetVoteComment(currentUserId, commentId);
            if (existedVote == null || existedVote.Vote == 0) return null;

            //update vote
            existedVote.Vote = 0;

            //update to database
            if (!_voteCommentRepository.Update(existedVote)) return null;

            var existedVoteDTO = _mapper.Map<VoteCommentDTO>(existedVote);

            var userDTO = _userHandlers.GetUser(currentUserId);
            existedVoteDTO.User = userDTO;

            var commentDTO = _commentHandlers.GetCommentBy(commentId);
            existedVoteDTO.Comment = commentDTO;

            return existedVoteDTO;
        }
    }
}
