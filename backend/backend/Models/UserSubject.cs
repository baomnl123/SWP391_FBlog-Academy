using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class UserSubject
    {
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public bool Status { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual User User { get; set; }
    }
}
