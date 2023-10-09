namespace backend.DTO
{
    public class UserDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
        public bool? IsAwarded { get; set; }
    }
}
