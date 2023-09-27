using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    [Table("post_list")]
    public class PostList
    {
        [Key]
        [Column("save_list_id")]
        [Required]
        public int SaveListId { get; set; }
        
        [Key]
        [Column("post_id")]
        [Required]
        public int PostId { get; set; }

        // Relation
        public SaveList SaveList { get; set; }
        public Post Post { get; set; }
    }
}
