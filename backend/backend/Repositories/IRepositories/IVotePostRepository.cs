using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVotePostRepository
    {
        //Get vote post
        public VotePost? GetVotePost(int currentUserId, int postId);

        //Get all users who vote specific post
        public ICollection<User>? GetAllUsersVotedBy(int postId);

        //CRUD vote post
        public bool Add(VotePost newVotePost);
        public bool Update(VotePost votePost);
        public bool DisableAllVotePostOf(Post post);
        public bool Save();
    }
}
