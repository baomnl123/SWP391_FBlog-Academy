using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Comment
    {
        public Comment()
        {
            VoteComments = new HashSet<VoteComment>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<VoteComment> VoteComments { get; set; }
    }
}
