using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostImageRepository : IPostImageRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostImageRepository()
        {
            _context = new();
        }

        public bool CreatePostImage(PostImage postImage)
        {
            _context.Add(postImage);
            return Save();
        }

        public bool DisablePostImage(PostImage postImage)
        {
            postImage.Status = false;
            _context.Update(postImage);
            return Save();
        }

        public bool EnablePostImage(PostImage postImage)
        {
            postImage.Status = true;
            _context.Update(postImage);
            return Save();
        }

        public bool UpdatePostImage(PostImage postImage)
        {
            _context.Update(postImage);
            return Save();
        }

        public ICollection<PostImage> GetPostImagesByImageId(int imageId)
        {
            return _context.PostImages.Where(c => c.ImageId == imageId).ToList();
        }

        public ICollection<PostImage> GetPostImagesByPostId(int postId)
        {
            return _context.PostImages.Where(c => c.PostId == postId).ToList();
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                // if saved > 0 then return true, else return false
                return saved > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}
