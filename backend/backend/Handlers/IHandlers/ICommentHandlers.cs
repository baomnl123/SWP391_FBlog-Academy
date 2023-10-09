using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface ICommentHandlers
    {
        //View Comments
        public ICollection<ICommentHandlers> ViewComments(int postId);
        //Create comment
        public CommentDTO CreateComment(int userId, int postId, string content);
        //Update comment
        public CommentDTO UpdateComment(int userId, int postId, string content);
        //Delete comment
        public bool DeleteComment(int userId, int postId);
    }
}
