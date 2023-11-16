
namespace backend.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public PostDTO? Post { get; set; }
        public UserDTO? User { get; set; }
        public string Content { get; set; }
        public int? Upvotes { get; set; } = 0;  
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int vote {  get; set; }
        public bool Status { get; set; }
    }
}
