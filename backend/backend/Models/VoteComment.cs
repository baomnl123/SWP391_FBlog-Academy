using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("VoteComment")]
    public partial class VoteComment
    {
        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        [NotNull]
        [Range(0,2)]
        public int Vote { get; set; }

        [NotNull]
        public DateTime CreateAt { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}
