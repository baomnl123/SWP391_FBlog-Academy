namespace backend.DTO
{
    public class SaveListDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool Status { get; set; }
    }
}
