using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("PostSubject")]
    public partial class PostSubject
    {
        [NotNull]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [NotNull]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
