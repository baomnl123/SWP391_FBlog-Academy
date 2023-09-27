using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("category_tag")]
    public class CategoryTag
    {
        [Key]
        [Column("tag_id")]
        [Required]
        public int TagId { get; set; }

        [Key]
        [Column("category_id")]
        [Required]
        public int CategoryId { get; set; }

        // Relation
        public Tag Tag { get; set; }
        public Category Category { get; set; }
    }
}
