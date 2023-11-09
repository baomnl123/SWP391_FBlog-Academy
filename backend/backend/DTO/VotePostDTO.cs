namespace backend.DTO
{
    public class VotePostDTO
    {
        public UserDTO? User { get; set; }
        public PostDTO? Post { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
