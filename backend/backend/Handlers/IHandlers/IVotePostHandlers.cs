using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IVotePostHandlers
    {
        //Create new vote post
        public VotePostDTO? CreateNewVotePost(int currentUserId, int postId, bool vote);
        //Update vote post
        public VotePostDTO? UpdateVotePost(int currentUserId, int postId, bool vote);
        //Get all users voted by postId
        public ICollection<UserDTO>? GetAllUsersVotedBy(int postId);
        //Dísable vote post
        public VotePostDTO? DisableVotePost(int currentUserId, int postId);
    }
}
