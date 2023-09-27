using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_FBlog.Models
{
    [Table("tag")]
    public class Tag
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("tag_name")]
        [Required]
        public string TagName { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("status")]
        [Required]
        public bool Status { get; set; }

        // Relation
        public User User { get; set; }
        public ICollection<CategoryTag> CategoryTags { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
