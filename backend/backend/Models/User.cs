using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace backend.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Majors = new HashSet<Major>();
            UserMajors = new HashSet<UserMajor>();
            Comments = new HashSet<Comment>();
            FollowUserFolloweds = new HashSet<FollowUser>();
            FollowUserFollowers = new HashSet<FollowUser>();
            PostReviewers = new HashSet<Post>();
            PostUsers = new HashSet<Post>();
            ReportPostAdmins = new HashSet<ReportPost>();
            ReportPostReporters = new HashSet<ReportPost>();
            SaveLists = new HashSet<SaveList>();
            Subjects = new HashSet<Subject>();
            UserSubjects = new HashSet<UserSubject>();
            VoteComments = new HashSet<VoteComment>();
            VotePosts = new HashSet<VotePost>();
        }

        [NotNull]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotNull]
        [MaxLength]
        public string AvatarUrl { get; set; }

        [NotNull]
        [MaxLength(50)]
        public string Name { get; set; }

        [NotNull]
        [MaxLength (50)]
        public string Email { get; set; }

        [NotNull]
        [MaxLength(65)]
        public string? Password { get; set; }

        [NotNull]
        [MaxLength(2)]
        public string Role { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [NotNull]
        public DateTime? UpdatedAt { get; set; }

        [NotNull]
        public bool Status { get; set; }

        [NotNull]
        public bool IsAwarded { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
        public virtual ICollection<UserMajor> UserMajors { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FollowUser> FollowUserFolloweds { get; set; }
        public virtual ICollection<FollowUser> FollowUserFollowers { get; set; }
        public virtual ICollection<Post> PostReviewers { get; set; }
        public virtual ICollection<Post> PostUsers { get; set; }
        public virtual ICollection<ReportPost> ReportPostAdmins { get; set; }
        public virtual ICollection<ReportPost> ReportPostReporters { get; set; }
        public virtual ICollection<SaveList> SaveLists { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<UserSubject> UserSubjects { get; set; }
        public virtual ICollection<VoteComment> VoteComments { get; set; }
        public virtual ICollection<VotePost> VotePosts { get; set; }
    }
}
