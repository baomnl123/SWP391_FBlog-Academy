using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class CommentHandlers : ICommentHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IVoteCommentRepository _voteCommentRepository;
        private readonly IPostRepository _postRepository;

        public CommentHandlers(IUserRepository userRepository, 
                                IPostRepository postRepository,
                                ICommentRepository commentRepository,
                                IVoteCommentRepository voteCommentRepository,
                                IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _commentRepository = commentRepository;
            _voteCommentRepository = voteCommentRepository;
            _postRepository = postRepository;
        }

        public CommentDTO? CreateComment(int userId, int postId, string content)
        {
            //return null if info needed to create is null
            if (content == null) return null;

            //return null if userId does not exist
            //                      or is removed
            //                      or does not have role SU(Student) or MOD(Moderator)
            var existedUser = _userRepository.GetUser(userId);
            if (existedUser == null || existedUser.Status == false
                || !( existedUser.Role.Contains("SU") || existedUser.Role.Contains("MOD") )) return null;

            //return null if post does not exist
            //                      or is removed
            //                      or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null
                || existedPost.Status == false
                || !(existedPost.Status == true && existedPost.IsApproved == true)) return null;

            //Create new Comment
            Comment newComment = new()
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.Now,
                Status = true,
            };

            //Add new comment to database
            if (!_commentRepository.Add(newComment)) return null;
            return _mapper.Map<CommentDTO>(newComment);
        }

        public CommentDTO? DeleteComment(int commentId)
        {
            //return null if comment does not exist
            //                          or is deleted
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedComment == null || existedComment.Status == false) return null;


            //return null and enable that comment again if disabling all vote of it is false
            if (!_voteCommentRepository.DisableAllVoteCommentOf(existedComment)) return null;

            //set status is false
            existedComment.Status = false;
            existedComment.UpdatedAt = DateTime.Now;

            //return null if updating to database is false
            if (!_commentRepository.Update(existedComment)) return null;


            //return deletedComment if deleting successful
            //                          and disable all vote of that comment
            return _mapper.Map<CommentDTO>(existedComment);
        }

        public CommentDTO? UpdateComment(int commentId, string content)
        {
            //return null if content is null
            if (content == null) return null;

            //return null if that comment does not exist
            //                              or is removed
            var existedComment = _commentRepository.GetComment(commentId);
            if (existedComment == null || existedComment.Status == false) return null;

            //update info of existedComment
            existedComment.Content = content;
            existedComment.UpdatedAt = DateTime.Now;

            //update to database
            if (!_commentRepository.Update(existedComment)) return null;
            return _mapper.Map<CommentDTO>(existedComment);
        }

        public ICollection<CommentDTO>? ViewAllComments(int postId)
        {
            // return null if post does not exist
            //                      or is removed
            //                      or is not approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || existedPost.Status == false 
                || !existedPost.IsApproved == true) return null;

            //Get All Comments of that post
            var existedComments = _commentRepository.ViewAllComments(postId);
            if (existedComments == null) return null;
            return _mapper.Map<List<CommentDTO>>(existedComments);
        }
    }
}
