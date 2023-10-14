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

        public bool DisableImage(PostImage postImage)
        {
            postImage.Status = false;
            _context.Update(postImage);
            return Save();
        }

        public bool EnableImage(PostImage postImage)
        {
            postImage.Status = true;
            _context.Update(postImage);
            return Save();
        }

        public ICollection<PostImage> GetPostImageByImageId(int imageId)
        {
            return _context.PostImages.Where(c => c.ImageId == imageId).ToList();
        }

        public ICollection<PostImage> GetPostImageByPostId(int postId)
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
