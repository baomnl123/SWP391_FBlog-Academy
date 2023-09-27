using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("post")]
    public class Post
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("reviewer_id")]
        [Required]
        public int ReviewerId { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("is_approved")]
        [Required]
        public bool IsApproved { get; set; }

        [Column("status")]
        [Required]
        public bool Status { get; set; }

        // Relation
        public User User { get; set; }
        public User Reviewer { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<VotePost> VotePosts { get; set; }
        public ICollection<PostList> PostLists { get; set; }
        public ICollection<PostCategory> PostCategories { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
