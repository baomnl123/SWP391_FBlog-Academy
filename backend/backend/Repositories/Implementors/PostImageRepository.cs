using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class PostImageRepository : IPostImageRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostImageRepository(FBlogAcademyContext context)
        {
            _context = new();
        }

        public Image? GetImagesById(int imageId)
        {
            return _context.Images.FirstOrDefault(c => c.Id == imageId);
        }

        public ICollection<PostImage> GetImagesByPost(int postId)
        {
            return _context.PostImages.Where(c => c.PostId == postId).ToList();
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
