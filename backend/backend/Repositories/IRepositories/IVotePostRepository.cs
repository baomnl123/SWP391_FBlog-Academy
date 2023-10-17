using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVotePostRepository
    {
        public VotePost? GetVotePost(int currentUserId, int postId);
        public ICollection<User>? GetAllUsersVotedBy(int postId);
        public bool Add(VotePost newVotePost);
        public bool Update(VotePost votePost);
        public bool DisableAllVotePostOf(Post post);
        public bool Save();
    }
}
