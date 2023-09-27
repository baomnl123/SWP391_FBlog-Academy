using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("post_category")]
    public class PostCategory
    {
        [Key]
        [Column("post_id")]
        [Required]
        public int PostId { get; set; }
        
        [Key]
        [Column("category_id")]
        [Required]
        public int CategoryId { get; set; }

        // Relation
        public Post Post { get; set; }
        public Category Category { get; set; }
    }
}
