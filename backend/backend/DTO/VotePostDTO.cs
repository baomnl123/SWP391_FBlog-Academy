namespace backend.DTO
{
    public class VotePostDTO
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
