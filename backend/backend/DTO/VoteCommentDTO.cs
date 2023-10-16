namespace backend.DTO
{
    public class VoteCommentDTO
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
