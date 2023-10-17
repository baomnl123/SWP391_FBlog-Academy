using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVotePostRepository
    {
        public VotePost? GetVotePost(int currentUserId, int postId);
        public ICollection<User>? GetAllUsersVotedBy(int postId);
        public VotePost? Add(VotePost newVotePost);
        public VotePost? Update(VotePost votePost);
        public VotePost? DisableAllVotePostOf(Post post);
        public bool Save();
    }
}
