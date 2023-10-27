using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Handlers.Implementors
{
    public class VoteCommentHandlers : IVoteCommentHandlers
    {
        private readonly IVoteCommentRepository _voteCommentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public VoteCommentHandlers(IVoteCommentRepository voteCommentRepository,
                                    IUserRepository userRepository,
                                    ICommentRepository commentRepository,
                                    IMapper mapper)
        {
            _voteCommentRepository = voteCommentRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public VoteCommentDTO? CreateVote(int currentUserId, int commentId, bool vote)
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
                if (existedVote.UpVote || existedVote.DownVote) return null;

                //
                if (vote)
                {
                    existedVote.UpVote = true;
                    existedVote.DownVote = false;
                    existedVote.CreateAt = DateTime.Now;
                }
                else
                {
                    existedVote.UpVote = false;
                    existedVote.DownVote = true;
                    existedVote.CreateAt = DateTime.Now;
                };
                if (!_voteCommentRepository.Update(existedVote)) return null;
                return _mapper.Map<VoteCommentDTO>(existedVote);
            }

            //Create new vote
            VoteComment newVote = new()
            {
                UserId = currentUserId,
                CommentId = commentId,
                CreateAt = DateTime.Now,
            };
            if (vote)
            {
                newVote.UpVote = true;
                newVote.DownVote = false;
                newVote.CreateAt = DateTime.Now;
            }
            else
            {
                newVote.UpVote = false;
                newVote.DownVote = true;
                newVote.CreateAt = DateTime.Now;
            };

            //Add new vote to database
            if (!_voteCommentRepository.Add(newVote)) return null;
            return _mapper.Map<VoteCommentDTO>(newVote);
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

        public VoteCommentDTO? UpdateVote(int currentUserId, int commentId, bool vote)
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

            //update to databse
            if (!_voteCommentRepository.Update(existedVote)) return null;
            return _mapper.Map<VoteCommentDTO>(existedVote);
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
            if (existedVote == null || (!existedVote.UpVote && !existedVote.DownVote)) return null;

            //update vote
            existedVote.UpVote = false;
            existedVote.DownVote = false;

            //update to database
            if (!_voteCommentRepository.Update(existedVote)) return null;
            return _mapper.Map<VoteCommentDTO>(existedVote);
        }
    }
}
