using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Image
    {
        public Image()
        {
            PostImages = new HashSet<PostImage>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}
