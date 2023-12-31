﻿using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface ICommentHandlers
    {
        //View All Comments
        public ICollection<CommentDTO>? ViewAllComments(int postId, int currentUserId);
        //Get Comment by comment's id
        public CommentDTO? GetCommentBy(int commentId);
        //Create comment
        public CommentDTO? CreateComment(int userId, int postId, string content);
        //Update comment
        public CommentDTO? UpdateComment(int commentId, string content);
        //Delete comment
        public CommentDTO? DeleteComment(int commentId);
    }
}
