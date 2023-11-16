using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace backend.Models
{
    public partial class VotePost
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        [Required]
        [Range(0, 2)]
        public int Vote { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
