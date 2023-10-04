using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostCategory
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Post Post { get; set; }
    }
}
