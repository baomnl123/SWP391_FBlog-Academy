using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Subject
    {
        public Subject()
        {
            UserSubjects = new HashSet<UserSubject>();
            MajorSubjects = new HashSet<MajorSubject>();
            PostSubjects = new HashSet<PostSubject>();
        }

        public int Id { get; set; }
        public int AdminId { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<UserSubject> UserSubjects { get; set; }
        public virtual ICollection<MajorSubject> MajorSubjects { get; set; }
        public virtual ICollection<PostSubject> PostSubjects { get; set; }
    }
}
