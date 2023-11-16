namespace backend.DTO
{
    public class MediaDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
