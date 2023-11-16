using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("MajorSubject")]
    public partial class MajorSubject
    {
        [NotNull]
        [Key]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [NotNull]
        [Key]
        [ForeignKey("Major")]
        public int MajorId { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Major Major { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
