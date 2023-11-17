using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace backend.Models
{
    public partial class FBlogAcademyContext : DbContext
    {
        public FBlogAcademyContext()
        {
        }

        public FBlogAcademyContext(DbContextOptions<FBlogAcademyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<MajorSubject> MajorSubjects { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FollowUser> FollowUsers { get; set; }
        public virtual DbSet<Media> Medias { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostMajor> PostMajors { get; set; }
        public virtual DbSet<PostList> PostLists { get; set; }
        public virtual DbSet<PostSubject> PostSubjects { get; set; }
        public virtual DbSet<ReportPost> ReportPosts { get; set; }
        public virtual DbSet<SaveList> SaveLists { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMajor> UserMajors { get; set; }
        public virtual DbSet<UserSubject> UserSubjects { get; set; }
        public virtual DbSet<VoteComment> VoteComments { get; set; }
        public virtual DbSet<VotePost> VotePosts { get; set; }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:FBlogDB"];
            return strConn;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Major>(entity =>
            {
                entity.HasIndex(e => e.MajorName)
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MajorName)
                    .IsRequired()
                    .HasColumnName("major_name");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<MajorSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.MajorId });

                /*entity.ToTable("MajorSubject");*/

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("subject_id");

                entity.Property(e => e.MajorId)
                    .IsRequired()
                    .HasColumnName("major_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.MajorSubjects)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMajorSu611140");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.MajorSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMajorSu484641");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                //entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKComment632288");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKComment400844");
            });

            modelBuilder.Entity<FollowUser>(entity =>
            {
                entity.HasKey(e => new { e.FollowerId, e.FollowedId });

                //entity.ToTable("FollowUser");

                entity.Property(e => e.FollowerId)
                    .IsRequired()
                    .HasColumnName("follower_id");

                entity.Property(e => e.FollowedId)
                    .IsRequired()
                    .HasColumnName("followed_id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Followed)
                    .WithMany(p => p.FollowUserFolloweds)
                    .HasForeignKey(d => d.FollowedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKFollowUser783758");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.FollowUserFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKFollowUser200833");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                //entity.ToTable("Media");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId)
                     .IsRequired()
                     .HasColumnName("post_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.MediaPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMedia400844");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                //entity.ToTable("Post");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsApproved)
                    .IsRequired()
                    .HasColumnName("is_approved");

                entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.PostReviewers)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FKPost548405");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPost990072");
            });

            modelBuilder.Entity<PostMajor>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.MajorId });

                /*entity.ToTable("PostMajor");*/

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.MajorId)
                    .IsRequired()
                    .HasColumnName("major_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.PostMajors)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostCatego136719");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostMajors)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostCatego519402");
            });

            modelBuilder.Entity<PostList>(entity =>
            {
                entity.HasKey(e => new { e.SaveListId, e.SavePostId });

                /*entity.ToTable("PostList");*/

                entity.Property(e => e.SaveListId)
                    .IsRequired()
                    .HasColumnName("save_list_id");

                entity.Property(e => e.SavePostId)
                    .IsRequired()
                    .HasColumnName("save_post_id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.SaveList)
                    .WithMany(p => p.PostLists)
                    .HasForeignKey(d => d.SaveListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostList365782");

                entity.HasOne(d => d.SavePost)
                    .WithMany(p => p.PostLists)
                    .HasForeignKey(d => d.SavePostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostList86128");
            });

            modelBuilder.Entity<PostSubject>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.SubjectId });

                /*entity.ToTable("PostSubject");*/

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("subject_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostSubjects)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostSubject716835");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.PostSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostSubject277175");
            });

            modelBuilder.Entity<ReportPost>(entity =>
            {
                entity.HasKey(e => new { e.ReporterId, e.PostId });

                /*entity.ToTable("ReportPost");*/

                entity.Property(e => e.ReporterId)
                    .IsRequired()
                    .HasColumnName("reporter_id");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.ReportPostAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FKReportPost712211");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKReportPost960109");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.ReportPostReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKReportPost680973");
            });

            modelBuilder.Entity<SaveList>(entity =>
            {
                //entity.ToTable("SaveList");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SaveLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKSaveList283598");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                //entity.ToTable("Subject");

                entity.HasIndex(e => e.SubjectName)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasColumnName("subject_name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<User>(entity =>
            {
                //entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__AB6E6164D120EBFA")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id");

                entity.Property(e => e.AvatarUrl)
                    .HasColumnName("avatar_url")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.IsAwarded)
                    .IsRequired()
                    .HasColumnName("is_awarded");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<UserMajor>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MajorId });

                /*entity.ToTable("UserMajor");*/

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.MajorId)
                    .IsRequired()
                    .HasColumnName("major_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.UserMajors)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKUserCatego136719");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMajors)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKUserCatego519402");
            });

            modelBuilder.Entity<UserSubject>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SubjectId });

                /*entity.ToTable("UserSubject");*/

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasColumnName("subject_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.UserSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKUserSubject136719");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKUserSubject519402");
            });

            modelBuilder.Entity<VoteComment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CommentId });

                /*entity.ToTable("VoteComment");*/

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.CommentId)
                    .IsRequired()
                    .HasColumnName("comment_id");

                entity.Property(e => e.CreateAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Vote)
                    .IsRequired()
                    .HasColumnName("vote");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.VoteComments)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKVoteCommen971268");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VoteComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKVoteCommen692680");
            });

            modelBuilder.Entity<VotePost>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId });

                /*entity.ToTable("VotePost");*/

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Vote)
                    .IsRequired()
                    .HasColumnName("vote");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.VotePosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKVotePost439037");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VotePosts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKVotePost207593");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
