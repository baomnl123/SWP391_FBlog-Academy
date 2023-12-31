﻿using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IVotePostHandlers
    {
        //Create new vote post
        public VotePostDTO? CreateNewVotePost(int currentUserId, int postId, int vote);
        //Update vote post
        public VotePostDTO? UpdateVotePost(int currentUserId, int postId, int vote);
        //Get all users voted by postId
        public ICollection<UserDTO>? GetAllUsersVotedBy(int postId);
        //Get all users downvoted by postId
        public ICollection<UserDTO>? GetAllUsersDownVotedBy(int postId);
        //Dísable vote post
        public VotePostDTO? DisableVotePost(int currentUserId, int postId);
    }
}
