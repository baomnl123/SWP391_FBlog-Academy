using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("Comment")]
    public partial class Comment
    {
        public Comment()
        {
            VoteComments = new HashSet<VoteComment>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [MaxLength]
        public string Content { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<VoteComment> VoteComments { get; set; }
    }
}
