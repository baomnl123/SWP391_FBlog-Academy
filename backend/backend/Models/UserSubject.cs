using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("UserSubject")]
    public partial class UserSubject
    {
        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual User User { get; set; }
    }
}
