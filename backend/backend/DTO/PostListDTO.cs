namespace backend.DTO
{
    public class PostListDTO
    {
        public SaveListDTO? SaveList { get; set; }
        public PostDTO? SavePost { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
