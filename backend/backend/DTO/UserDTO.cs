namespace backend.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string Role { get; set; }
        public int successReportedTimes { get; set; } = 0;
        public int followerNumber { get; set; } = 0;
        public ICollection<UserDTO>? followersList { get; set; }
        public int postNumber { get; set; } = 0;
        public ICollection<PostDTO>? postsList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
        public bool? IsAwarded { get; set; }
        public bool isFollowed { get; set; } = false;
    }
}
