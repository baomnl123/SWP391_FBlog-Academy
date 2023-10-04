using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class ReportPost
    {
        public int ReporterId { get; set; }
        public int PostId { get; set; }
        public int AdminId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Admin { get; set; }
        public virtual Post Post { get; set; }
        public virtual User Reporter { get; set; }
    }
}
