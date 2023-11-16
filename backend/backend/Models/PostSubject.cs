using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostSubject
    {
        public int PostId { get; set; }
        public int SubjectId { get; set; }
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
