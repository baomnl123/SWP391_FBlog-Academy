namespace backend.DTO
{
    public class VoteCommentDTO
    {
        public UserDTO? User { get; set; }
        public CommentDTO? Comment { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
