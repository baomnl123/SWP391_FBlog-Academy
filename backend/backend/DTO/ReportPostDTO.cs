namespace backend.DTO
{
    public class ReportPostDTO
    {
        public int ReporterId { get; set; }
        public int PostId { get; set; }
        public int AdminId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
