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
        [ForeignKey("SaveList")]
        public int SaveListId { get; set; }

        [NotNull]
        [ForeignKey("SavePost")]
        public int SavePostId { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual SaveList SaveList { get; set; }
        public virtual Post SavePost { get; set; }
    }
}
