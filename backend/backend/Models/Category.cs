using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryTags = new HashSet<CategoryTag>();
            PostCategories = new HashSet<PostCategory>();
        }

        public int Id { get; set; }
        public int AdminId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<CategoryTag> CategoryTags { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
    }
}
