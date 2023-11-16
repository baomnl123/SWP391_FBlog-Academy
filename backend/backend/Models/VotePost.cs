using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("VotePost")]
    public partial class VotePost
    {
        [NotNull]
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [Key]
        public int PostId { get; set; }

        [NotNull]
        [Range(0, 2)]
        public int Vote { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
