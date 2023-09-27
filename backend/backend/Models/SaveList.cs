using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("save_list")]
    public class SaveList
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId{ get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("status")]
        [Required]
        public bool Status{ get; set; }

        // Relation
        public User User { get; set; }
        public ICollection<PostList> PostLists { get; set; }
    }
}
