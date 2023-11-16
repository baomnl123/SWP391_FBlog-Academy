using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Major
    {
        public Major()
        {
            UserMajors = new HashSet<UserMajor>();
            MajorSubjects = new HashSet<MajorSubject>();
            PostMajors = new HashSet<PostMajor>();
        }

        public int Id { get; set; }
        public int AdminId { get; set; }
        public string MajorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<UserMajor> UserMajors { get; set; }
        public virtual ICollection<MajorSubject> MajorSubjects { get; set; }
        public virtual ICollection<PostMajor> PostMajors { get; set; }
    }
}
