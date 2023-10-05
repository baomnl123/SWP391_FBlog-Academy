using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class VotePost
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public bool? UpVote { get; set; }
        public bool? DownVote { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
