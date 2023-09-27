using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_FBlog.Models
{
    [Table("vote_post")]
    public class VotePost
    {
        [Key]
        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("post_id")]
        [Required]
        public int PostId { get; set; }

        [Column("up_vote")]
        public bool UpVote { get; set; }

        [Column("down_vote")]
        public bool DownVote { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Relation
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
