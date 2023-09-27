using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("follow_user")]
    public class FollowUser
    {
        [Key]
        [Column("follower_id")]
        [Required]
        public int FollowerId { get; set; }

        [Key]
        [Column("followed_id")]
        [Required]
        public int FollowedId { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("status")]
        [Required]
        public bool status { get; set; }
    }
}
