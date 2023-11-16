using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("PostMajor")]
    public partial class PostMajor
    {
        [NotNull]
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [NotNull]
        [Key]
        [ForeignKey("Major")]
        public int MajorId { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Major Major { get; set; }
        public virtual Post Post { get; set; }
    }
}
