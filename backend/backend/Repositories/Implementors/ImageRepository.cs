using backend.Models;
using backend.Repositories.IRepositories;

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

        public bool CreateImage(Image image)
        {
            _context.Add(image);
            return Save();
        }

        public bool UpdateImage(Image image)
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
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}