using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("FollowUser")]
    public partial class FollowUser
    {
        [NotNull]
        [ForeignKey("Follower")]
        public int FollowerId { get; set; }

        [NotNull]
        [ForeignKey("Followed")]
        public int FollowedId { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual User Followed { get; set; }
        public virtual User Follower { get; set; }
    }
}
