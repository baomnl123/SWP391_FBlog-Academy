using System;
using System.Collections.Generic;

namespace backend.Models
{
    public partial class PostImage
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Post Post { get; set; }
    }
}
