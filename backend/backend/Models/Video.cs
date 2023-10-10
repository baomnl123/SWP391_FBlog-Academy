using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Video
    {
        public Video()
        {
            PostVideos = new HashSet<PostVideo>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<PostVideo> PostVideos { get; set; }
    }
}
