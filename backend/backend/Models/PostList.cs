using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class PostList
    {
        public int SaveListId { get; set; }
        public int SavePostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }

        public virtual SaveList SaveList { get; set; }
        public virtual Post SavePost { get; set; }
    }
}
