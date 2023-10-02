using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class SaveList
    {
        public SaveList()
        {
            PostLists = new HashSet<PostList>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PostList> PostLists { get; set; }
    }
}
