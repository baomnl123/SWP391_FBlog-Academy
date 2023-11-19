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
        public int majorsNumber { get; set; } = 0;
        public int subjectsNumber { get; set; } = 0;
        public int followerNumber { get; set; } = 0;
        public int followingNumber { get; set; } = 0;
        public int postNumber { get; set; } = 0;
        public ICollection<MajorDTO>? majorsList { get; set; } = new List<MajorDTO>();
        public ICollection<SubjectDTO>? subjectsList { get; set; } = new List<SubjectDTO>();
        public ICollection<UserDTO>? followersList { get; set; } = new List<UserDTO>();
        public ICollection<UserDTO>? followingsList { get; set; } = new List<UserDTO>();
        public ICollection<PostDTO>? postsList { get; set; } = new List<PostDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
        public bool? IsAwarded { get; set; }
        public bool isFollowed { get; set; } = false;
    }
}
