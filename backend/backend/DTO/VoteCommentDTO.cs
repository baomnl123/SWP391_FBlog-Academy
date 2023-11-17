namespace backend.DTO
{
    public class VoteCommentDTO
    {
        public UserDTO? User { get; set; }
        public CommentDTO? Comment { get; set; }
        public int Vote { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
