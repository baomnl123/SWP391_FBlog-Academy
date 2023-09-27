using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("category")]
    public class Category
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("category_name")]
        [Required]
        public string CategoryName { get; set; }

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
        public ICollection<PostCategory> PostCategories { get; set; }
    }
}
