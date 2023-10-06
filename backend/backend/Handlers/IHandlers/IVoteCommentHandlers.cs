namespace backend.Handlers.IHandlers
{
    public interface IVoteCommentHandlers
    {
        //Vote another comment
        public bool UpVoteOtherCommnet(int currentUserId, int commentId);
        //Dísable upvote comment
        public bool DisableUpVoteOtherCommnet(int currentUserId, int commentId);
        //Downvote another post
        public bool DownVoteOtherComment(int currentUserId, int commentId);
        //Disable downvote comment
        public bool DisableDownVoteOtherComment(int currentUserId, int commentId);
    }
}
