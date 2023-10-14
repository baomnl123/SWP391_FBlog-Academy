using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        public ICollection<Comment>? ViewAllComments(int postId);
        public bool Add(Comment newComment);
        public bool Save();
    }
}
