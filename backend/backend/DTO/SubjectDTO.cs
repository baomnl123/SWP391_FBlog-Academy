namespace backend.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string? SubjectName { get; set; }
        public ICollection<MajorDTO>? Major { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}
