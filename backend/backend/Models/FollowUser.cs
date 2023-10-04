using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class FollowUser
    {
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual User Followed { get; set; }
        public virtual User Follower { get; set; }
    }
}
