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

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryTag> CategoryTags { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FollowUser> FollowUsers { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<PostList> PostLists { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
        public virtual DbSet<ReportPost> ReportPosts { get; set; }
        public virtual DbSet<SaveList> SaveLists { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<VoteComment> VoteComments { get; set; }
        public virtual DbSet<VotePost> VotePosts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server= (local);uid=sa;pwd=12345;database=FBlogAcademy;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasIndex(e => e.CategoryName)
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.CategoryName)
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
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCategory311908");
            });

            modelBuilder.Entity<CategoryTag>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.CategoryId })
                    .HasName("PK__Category__1FC24C2D619EB0CA");

                entity.ToTable("CategoryTag");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryTags)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCategoryTa611140");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.CategoryTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCategoryTa484641");
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

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.CategoryId })
                    .HasName("PK__PostCate__638369FD4FBDCD1A");

                entity.ToTable("PostCategory");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PostCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostCatego136719");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostCategories)
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

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.TagId })
                    .HasName("PK__PostTag__4AFEED4D0915C090");

                entity.ToTable("PostTag");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostTag716835");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKPostTag277175");
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

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.HasIndex(e => e.TagName)
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("tag_name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKTag383419");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__AB6E6164D120EBFA")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id");

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

                entity.Property(e => e.DownVote).HasColumnName("down_vote");

                entity.Property(e => e.UpVote).HasColumnName("up_vote");

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
