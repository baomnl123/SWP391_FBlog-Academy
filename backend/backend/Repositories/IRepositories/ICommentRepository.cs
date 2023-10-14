using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        public Comment? GetComment(int commentId);
        public ICollection<Comment>? ViewAllComments(int postId);
        public bool Add(Comment newComment);
        public bool Save();
    }
}
