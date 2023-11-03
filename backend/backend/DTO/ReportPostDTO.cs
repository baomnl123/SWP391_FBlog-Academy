namespace backend.DTO
{
    public class ReportPostDTO
    {
        public UserDTO? Reporter { get; set; }
        public PostDTO? Post { get; set; }
        public UserDTO? Admin { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
