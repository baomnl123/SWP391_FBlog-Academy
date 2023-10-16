using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IVoteCommentHandlers
    {
        //Get all users by comment
        public ICollection<UserDTO>? GetAllUsersBy(int commentId);
        //Create new Vote
        public VoteCommentDTO? CreateVote(int currentUserId, int commentId, bool vote);
        //Update vote 
        public VoteCommentDTO? UpdateVote(int currentUserId, int commentId, bool vote);
        //Disable vote 
        public VoteCommentDTO? DisableVote(int currentUserId, int commentId);
    }
}
