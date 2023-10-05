using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Tag
    {
        public Tag()
        {
            CategoryTags = new HashSet<CategoryTag>();
            PostTags = new HashSet<PostTag>();
        }

        public int Id { get; set; }
        public int AdminId { get; set; }
        public string TagName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<CategoryTag> CategoryTags { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
