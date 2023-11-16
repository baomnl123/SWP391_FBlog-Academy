using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("PostList")]
    public partial class PostList
    {
        [NotNull]
        [Key]
        [ForeignKey("SaveList")]
        public int SaveListId { get; set; }

        [NotNull]
        [Key]
        [ForeignKey("")]
        public int SavePostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual SaveList SaveList { get; set; }
        public virtual Post SavePost { get; set; }
    }
}
