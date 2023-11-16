using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("Post")]
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            PostMajors = new HashSet<PostMajor>();
            PostLists = new HashSet<PostList>();
            PostSubjects = new HashSet<PostSubject>();
            ReportPosts = new HashSet<ReportPost>();
            VotePosts = new HashSet<VotePost>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [NotNull]
        [ForeignKey("User")]
        public int? ReviewerId { get; set; }

        [NotNull]
        [MaxLength(100)]
        public string Title { get; set; }

        [NotNull]
        [MaxLength]
        public string Content { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public DateTime? UpdatedAt { get; set; }

        [NotNull]
        public bool IsApproved { get; set; }

        [NotNull]
        public bool Status { get; set; }

        public virtual User Reviewer { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> ImagePosts { get; set; }
        public virtual ICollection<Video> VideoPosts { get; set; }
        public virtual ICollection<PostMajor> PostMajors { get; set; }
        public virtual ICollection<PostList> PostLists { get; set; }
        public virtual ICollection<PostSubject> PostSubjects { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<VotePost> VotePosts { get; set; }
    }
}
