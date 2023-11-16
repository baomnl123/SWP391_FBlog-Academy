using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("SaveList")]
    public partial class SaveList
    {
        public SaveList()
        {
            PostLists = new HashSet<PostList>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [MaxLength]
        public string Name { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public DateTime? UpdateAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PostList> PostLists { get; set; }
    }
}
