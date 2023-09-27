using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column ("email")]
        [Required]
        public string? Email { get; set; }

        [Column("password")]
        [Required]
        public string Password { get; set; }

        [Column("role_id")]
        [Required]
        public int RoleId { get; set; }
        
        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("status")]
        [Required]
        public bool Status { get; set; }

        [Column("is_awarded")]
        [Required]
        public bool IsAwarded{ get; set; }

        // Relation
        // FollowUser, ReportUser table
        public Role Role { get; set; }
        public ICollection<SaveList> SaveLists { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts{ get; set; }
        public ICollection<Post> ReviewPosts{ get; set; }
        public ICollection<VotePost> VotePosts { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
