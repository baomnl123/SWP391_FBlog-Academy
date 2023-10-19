using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ICommentRepository
    {
        //Get comment
        public Comment? GetComment(int commentId);
        public ICollection<Comment>? ViewAllComments(int postId);
        //CRUD comment
        public bool Add(Comment newComment);
        public bool Update(Comment comment);
        public bool Save();
    }
}
