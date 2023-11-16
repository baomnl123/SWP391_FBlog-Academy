using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("ReportPost")]
    public partial class ReportPost
    {
        [NotNull]
        [Key]
        [ForeignKey("User")]
        public int ReporterId { get; set; }

        [NotNull]
        [Key]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [NotNull]
        [ForeignKey("User")]
        public int? AdminId { get; set; }

        [NotNull]
        [MaxLength(255)]
        public string Content { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        [MaxLength(10)]
        public string Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual Post Post { get; set; }
        public virtual User Reporter { get; set; }
    }
}
