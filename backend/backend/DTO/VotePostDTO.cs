namespace backend.DTO
{
    public class VotePostDTO
    {
        public UserDTO? User { get; set; }
        public PostDTO? Post { get; set; }
        public int Vote { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
