using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("Major")]
    public partial class Major
    {
        public Major()
        {
            UserMajors = new HashSet<UserMajor>();
            MajorSubjects = new HashSet<MajorSubject>();
            PostMajors = new HashSet<PostMajor>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [MaxLength(50)]
        public string MajorName { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public DateTime? UpdatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<UserMajor> UserMajors { get; set; }
        public virtual ICollection<MajorSubject> MajorSubjects { get; set; }
        public virtual ICollection<PostMajor> PostMajors { get; set; }
    }
}
