namespace backend.Handlers.IHandlers
{
    public interface IVoteCommentHandlers
    {
        //Vote another comment
        public bool UpVoteOtherCommnet(int currentUserId, int postId);
        //Downvote another post
        public bool DownVoteOtherComment(int currentUserId, int postId);
    }
}
