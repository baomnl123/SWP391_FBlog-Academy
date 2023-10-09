namespace backend.DTO
{
    public class PostVideoDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
