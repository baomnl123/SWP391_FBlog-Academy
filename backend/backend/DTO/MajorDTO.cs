namespace backend.DTO
{
    public class MajorDTO
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string? MajorName { get; set; }
        public List<SubjectDTO>? Subjects { get; set; } = new List<SubjectDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
    }
}
