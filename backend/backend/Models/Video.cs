using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Video
    {
        public Video()
        {
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual Post Post { get; set; }
    }
}
