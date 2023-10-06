namespace backend.Handlers.IHandlers
{
    public interface IVotePostHandlers
    {
        //Vote another post
        public bool UpVoteOtherPost(int currentUserId, int postId);
        //Downvote another post
        public bool DownVoteOtherPost(int currentUserId, int postId);
    }
}
