namespace backend.DTO
{
    public class FollowUserDTO
    {
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
