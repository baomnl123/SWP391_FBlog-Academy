using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostImage
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }
        public bool Status { get; set; }

        public virtual Image Image { get; set; }
        public virtual Post Post { get; set; }
    }
}
