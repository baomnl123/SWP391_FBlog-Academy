namespace backend.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public UserDTO? User { get; set; }
        public int? ReviewerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Upvotes { get; set; } = 0;
        public int? Downvotes { get; set; } = 0;
        public ICollection<UserDTO>? UsersUpvote { get; set; } = new List<UserDTO>();
        public ICollection<UserDTO>? UsersDownvote { get; set; } = new List<UserDTO>();
        public ICollection<MediaDTO>? Medias { get; set; } = new List<MediaDTO>();
        public ICollection<MajorDTO>? Majors { get; set; } = new List<MajorDTO>();
        public ICollection<SubjectDTO>? Subjects { get; set; } = new List<SubjectDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Vote { get; set; } = 0;
        public bool IsApproved { get; set; }
        public bool Status { get; set; }
    }
}
