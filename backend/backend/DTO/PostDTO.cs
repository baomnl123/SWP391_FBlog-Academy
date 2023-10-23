namespace backend.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ReviewerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<VideoDTO>? Videos { get; set; }
        public ICollection<ImageDTO>? Images { get; set; }
        public ICollection<CategoryDTO>? categories { get; set; }
        public ICollection<TagDTO>? tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }
        public bool Status { get; set; }
    }
}
