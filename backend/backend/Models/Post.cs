﻿using System;
using System.Collections.Generic;

#nullable disable

namespace backend.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            PostCategories = new HashSet<PostCategory>();
            PostLists = new HashSet<PostList>();
            PostTags = new HashSet<PostTag>();
            ReportPosts = new HashSet<ReportPost>();
            VotePosts = new HashSet<VotePost>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ReviewerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }
        public bool Status { get; set; }

        public virtual User Reviewer { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> ImagePosts { get; set; }
        public virtual ICollection<Video> VideoPosts { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
        public virtual ICollection<PostList> PostLists { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
        public virtual ICollection<VotePost> VotePosts { get; set; }
    }
}
