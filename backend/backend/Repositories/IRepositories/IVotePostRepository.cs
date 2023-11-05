using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVotePostRepository
    {
        //Get vote post
        public VotePost? GetVotePost(int currentUserId, int postId);

        //Get all users who upvote specific post
        public ICollection<User>? GetAllUsersVotedBy(int postId);
        //Get all users who downvote specific post
        public ICollection<User>? GetAllUsersDownVotedBy(int postId);

        //CRUD vote post
        public bool Add(VotePost newVotePost);
        public bool Update(VotePost votePost);
        public bool DisableAllVotePostOf(Post post);
        public bool Save();
    }
}
