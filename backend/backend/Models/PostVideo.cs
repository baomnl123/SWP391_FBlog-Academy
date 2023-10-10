using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostVideo
    {
        public int VideoId { get; set; }
        public int PostId { get; set; }
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual Video Video { get; set; }
    }
}
