using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class VoteComment
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public bool UpVote { get; set; }
        public bool DownVote { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}
