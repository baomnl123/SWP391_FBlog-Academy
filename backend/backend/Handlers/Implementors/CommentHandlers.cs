﻿using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;
using backend.Utils;

namespace backend.Handlers.Implementors
{
    public class CommentHandlers : ICommentHandlers
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IVoteCommentRepository _voteCommentRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserRoleConstrant _userRoleConstrant;

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
            _userRoleConstrant = new UserRoleConstrant();
        }

        public CommentDTO? CreateComment(int userId, int postId, string content)
        {
            //return null if info needed to create is null
            if (content == null) return null;

            //return null if userId does not exist
            //                      or is removed
            //                      or does not have role SU(Student) or MOD(Moderator)
            var existedUser = _userRepository.GetUser(userId);
            var studentRole = _userRoleConstrant.GetStudentRole();
            var modRole = _userRoleConstrant.GetModeratorRole();
            if (existedUser == null || existedUser.Status == false
                || !( existedUser.Role.Contains(studentRole) || existedUser.Role.Contains(modRole) )) return null;

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

            var newCommentDTO = _mapper.Map<CommentDTO>(newComment);

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByCommentID(newCommentDTO.Id));
            newCommentDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            return newCommentDTO;
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


            var existedCommentDTO = _mapper.Map<CommentDTO>(existedComment);

            var commentUpvote = _voteCommentRepository.GetAllUserBy(existedCommentDTO.Id);
            existedCommentDTO.Upvotes = (commentUpvote == null || commentUpvote.Count == 0) ? 0 : commentUpvote.Count;

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByCommentID(existedCommentDTO.Id));
            existedCommentDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            //return deletedComment if deleting successful
            //                          and disable all vote of that comment
            return existedCommentDTO;
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

            var existedCommentDTO = _mapper.Map<CommentDTO>(existedComment);

            var commentUpvote = _voteCommentRepository.GetAllUserBy(existedCommentDTO.Id);
            existedCommentDTO.Upvotes = (commentUpvote == null || commentUpvote.Count == 0) ? 0 : commentUpvote.Count;

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByCommentID(existedCommentDTO.Id));
            existedCommentDTO.User = (getUser is not null && getUser.Status) ? getUser : null;

            return existedCommentDTO;
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
            if (existedComments == null || existedComments.Count == 0) return null;

            var existedCommentsDTO = _mapper.Map<List<CommentDTO>>(existedComments);

            foreach(var comment in existedCommentsDTO)
            {
                var commentUpvote = _voteCommentRepository.GetAllUserBy(comment.Id);
                comment.Upvotes = (commentUpvote == null || commentUpvote.Count == 0) ? 0 : commentUpvote.Count;

                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUserByCommentID(comment.Id));
                comment.User = (getUser is not null && getUser.Status) ? getUser : null;
            }

            return existedCommentsDTO;
        }
    }
}
