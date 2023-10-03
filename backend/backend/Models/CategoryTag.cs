using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class CategoryTag
    {
        public int TagId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
