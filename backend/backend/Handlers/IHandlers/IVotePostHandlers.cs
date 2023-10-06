namespace backend.Handlers.IHandlers
{
    public interface IVotePostHandlers
    {
        //Vote another post
        public bool UpVoteOtherPost(int currentUserId, int postId);
        //Dísable upvote post
        public bool DisableUpVoteOtherPost(int currentUserId, int postId);
        //Downvote another post
        public bool DownVoteOtherPost(int currentUserId, int postId);
        //Dísable downvote post
        public bool DisableDownVoteOtherPost(int currentUserId, int postId);
    }
}
