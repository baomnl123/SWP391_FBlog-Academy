﻿using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IVoteCommentHandlers
    {
        //Get all users voted by comment
        public ICollection<UserDTO>? GetAllUsersVotedBy(int commentId);
        //Create new Vote
        public VoteCommentDTO? CreateVote(int currentUserId, int commentId, int vote);
        //Update vote 
        public VoteCommentDTO? UpdateVote(int currentUserId, int commentId, int vote);
        //Disable vote 
        public VoteCommentDTO? DisableVote(int currentUserId, int commentId);
    }
}
