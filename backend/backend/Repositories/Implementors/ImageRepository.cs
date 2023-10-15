using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class ImageRepository : IImageRepository
    {
        private readonly FBlogAcademyContext _context;

        public ImageRepository()
        {
            _context = new();
        }

        public Image? GetImageById(int imageId)
        {
            return _context.Images.FirstOrDefault(c => c.Id == imageId);
        }

        public Image? GetImageByURL(string imageURL)
        {
            return _context.Images.FirstOrDefault(c => c.Url == imageURL);
        }

        public ICollection<Image> GetImagesByPost(int postId)
        {
            return _context.PostImages.Where(e => e.PostId == postId && e.Status == true)
                                      .Select(e => e.Image)
                                      .Where(c => c.Status == true).ToList();
        }

        public bool CreateImage(int postId, Image image)
        {
            var post = _context.Posts.FirstOrDefault(c => c.Id == postId && c.Status == true);

            if (post == null) return false;
            // Add PostImage
            var postImage = new PostImage()
            {
                Image = image,
                Post = post
            };
            _context.Add(postImage);

            _context.Add(image);
            return Save();
        }

        public bool UpdateImage(int postId, Image image)
        {
            _context.Update(image);
            return Save();
        }

        public bool DisableImage(Image image)
        {
            image.Status = false;
            _context.Remove(image);
            return Save();
        }

        public bool EnableImage(Image image)
        {
            image.Status = true;
            _context.Update(image);
            return Save();
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