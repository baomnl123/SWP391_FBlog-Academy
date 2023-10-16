using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVoteCommentRepository
    {
        //get vote comment
        public VoteComment? GetVoteComment(int userId, int commentId);
        //get all users by commentId
        public ICollection<User>? GetAllUserBy(int commentId);

        //CRUD vote comment
        public bool Add(VoteComment comment);
        public bool Update(VoteComment comment);
        public bool DisableAllVoteCommentOf(Comment comment);
        public bool Save();
    }
}
