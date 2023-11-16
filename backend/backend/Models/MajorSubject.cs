using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class MajorSubject
    {
        public int SubjectId { get; set; }
        public int MajorId { get; set; }
        public bool Status { get; set; }

        public virtual Major Major { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
