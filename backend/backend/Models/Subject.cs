using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("Subject")]
    public partial class Subject
    {
        public Subject()
        {
            UserSubjects = new HashSet<UserSubject>();
            MajorSubjects = new HashSet<MajorSubject>();
            PostSubjects = new HashSet<PostSubject>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [MaxLength(20)]
        public string SubjectName { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public DateTime? UpdatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<UserSubject> UserSubjects { get; set; }
        public virtual ICollection<MajorSubject> MajorSubjects { get; set; }
        public virtual ICollection<PostSubject> PostSubjects { get; set; }
    }
}
