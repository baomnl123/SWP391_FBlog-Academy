using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class UserTag
    {
        public int UserId { get; set; }
        public int TagId { get; set; }
        public bool Status { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual User User { get; set; }
    }
}
