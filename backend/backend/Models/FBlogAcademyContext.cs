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
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostMajor> PostMajors { get; set; }
        public virtual DbSet<PostList> PostLists { get; set; }
        public virtual DbSet<PostSubject> PostSubjects { get; set; }
        public virtual DbSet<ReportPost> ReportPosts { get; set; }
        public virtual DbSet<SaveList> SaveLists { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMajor> UserMajors { get; set; }
        public virtual DbSet<UserSubject> UserSubjects{ get; set; }
        public virtual DbSet<Video> Videos { get; set; }
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
                entity.ToTable("Major");

                entity.HasIndex(e => e.MajorName)
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.MajorName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("category_name");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Majors)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMajor311908");
            });

            modelBuilder.Entity<MajorSubject>(entity =>
            {
                entity.HasKey(e => new { e.SubjectId, e.MajorId })
                    .HasName("PK__Major__1FC24C2D619EB0CA");

                entity.ToTable("MajorSubject");

                entity.Property(e => e.SubjectId).HasColumnName("tag_id");

                entity.Property(e => e.MajorId).HasColumnName("category_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.MajorSubjects)
                    .HasForeignKey(d => d.MajorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMajorTa611140");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.MajorSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKMajorTa484641");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

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
                entity.HasKey(e => new { e.FollowerId, e.FollowedId })
                    .HasName("PK__FollowUs__838707A38FAF7E83");

                entity.ToTable("FollowUser");

                entity.Property(e => e.FollowerId).HasColumnName("follower_id");

                entity.Property(e => e.FollowedId).HasColumnName("followed_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

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

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId)
                     .IsRequired()
                     .HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ImagePosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKImage400844");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsApproved).HasColumnName("is_approved");

                entity.Property(e => e.ReviewerId).HasColumnName("reviewer_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

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
                entity.HasKey(e => new { e.PostId, e.MajorId })
                    .HasName("PK__PostCate__638369FD4FBDCD1A");

                entity.ToTable("PostMajor");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.MajorId).HasColumnName("category_id");

                entity.Property(e => e.Status).HasColumnName("status");

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
                entity.HasKey(e => new { e.SaveListId, e.SavePostId })
                    .HasName("PK__PostList__859A5D1BB3774C7B");

                entity.ToTable("PostList");

                entity.Property(e => e.SaveListId).HasColumnName("save_list_id");

                entity.Property(e => e.SavePostId).HasColumnName("save_post_id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

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
                entity.HasKey(e => new { e.PostId, e.SubjectId })
                    .HasName("PK__PostSubject__4AFEED4D0915C090");

                entity.ToTable("PostSubject");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.SubjectId).HasColumnName("tag_id");

                entity.Property(e => e.Status).HasColumnName("status");

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
                entity.HasKey(e => new { e.ReporterId, e.PostId })
                    .HasName("PK__ReportPo__39154D75DD3D3CC7");

                entity.ToTable("ReportPost");

                entity.Property(e => e.ReporterId).HasColumnName("reporter_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("status")
                    .IsFixedLength(true);

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
                entity.ToTable("SaveList");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SaveLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKSaveList283598");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.HasIndex(e => e.SubjectName)
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("tag_name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKSubject383419");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__AB6E6164D120EBFA")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.AvatarUrl)
                    .HasColumnName("avatar_url")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("email");

                entity.Property(e => e.IsAwarded).HasColumnName("is_awarded");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(65)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("role")
                    .IsFixedLength(true);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<UserMajor>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MajorId })
                    .HasName("PK__UserCate__638369FD4FBDCD1A");

                entity.ToTable("UserMajor");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.MajorId).HasColumnName("category_id");

                entity.Property(e => e.Status).HasColumnName("status");

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
                entity.HasKey(e => new { e.UserId, e.SubjectId })
                    .HasName("PK__UserSubject__638369FD4FBDCD1A");

                entity.ToTable("UserSubject");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.SubjectId).HasColumnName("tag_id");

                entity.Property(e => e.Status).HasColumnName("status");

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

            modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("Video");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.VideoPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKVideo280203");
            });

            modelBuilder.Entity<VoteComment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CommentId })
                    .HasName("PK__VoteComm__D7C76067C06255A7");

                entity.ToTable("VoteComment");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.DownVote).HasColumnName("down_vote");

                entity.Property(e => e.UpVote).HasColumnName("up_vote");

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
                entity.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("PK__VotePost__CA534F79E6329B08");

                entity.ToTable("VotePost");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Vote).HasColumnName("vote");

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
