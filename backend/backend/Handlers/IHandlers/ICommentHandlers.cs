using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface ICommentHandlers
    {
        //View Comments
        public ICollection<ICommentHandlers> ViewComments();
        //Create comments
        public CommentDTO CreateComment(int userId, string content);
        //Update comments
        public CommentDTO UpdateComment(int userId, string content);
        //Delete comments
        public CommentDTO DeleteComment(int userId);
        //vote comments
        public VoteCommentDTO VoteComment(int userId, int postId, bool vote);
    }
}
