namespace backend.DTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string TagName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}
