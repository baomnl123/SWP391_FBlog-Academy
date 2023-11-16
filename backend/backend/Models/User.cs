using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
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

        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }
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
