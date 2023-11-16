using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("Media")]
    public partial class Media
    {
        public Media()
        {
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [NotNull]
        [MaxLength(5)]
        public string Type { get; set; }

        [NotNull]
        [MaxLength]
        public string Url { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
    }
}
