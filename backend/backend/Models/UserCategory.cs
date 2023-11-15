using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class UserCategory
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
