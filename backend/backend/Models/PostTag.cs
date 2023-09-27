using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("post_tag")]
    public class PostTag
    {
        [Key]
        [Column("post_id")]
        [Required]
        public int PostId { get; set; }
        
        [Key]
        [Column("tag_id")]
        [Required]
        public int TagId { get; set; }

        // Relation
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
