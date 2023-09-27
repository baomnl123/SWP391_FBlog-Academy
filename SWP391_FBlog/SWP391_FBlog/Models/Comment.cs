using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("post_id")]
        [Required]
        public int PostId { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("content")]
        [Required]
        public string Content { get; set; }

        [Column("status")]
        [Required]
        public bool Status { get; set; }

        // Relation
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
