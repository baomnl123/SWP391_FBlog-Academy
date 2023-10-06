using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface ICommentHandlers
    {
        //View Comments
        public ICollection<ICommentHandlers> ViewComments();
        //Create comment
        public CommentDTO CreateComment(int userId, string content);
        //Update comment
        public CommentDTO UpdateComment(int userId, string content);
        //Delete comment
        public bool DeleteComment(int userId);
    }
}
