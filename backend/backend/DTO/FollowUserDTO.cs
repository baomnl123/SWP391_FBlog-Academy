namespace backend.DTO
{
    public class FollowUserDTO
    {
        public UserDTO Follower { get; set; }
        public UserDTO Followed { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
