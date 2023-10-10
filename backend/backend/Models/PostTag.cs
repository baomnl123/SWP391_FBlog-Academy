using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostTag
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
