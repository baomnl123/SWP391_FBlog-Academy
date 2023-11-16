using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("UserMajor")]
    public partial class UserMajor
    {
        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [ForeignKey("Major")]
        public int MajorId { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Major Major { get; set; }
        public virtual User User { get; set; }
    }
}
