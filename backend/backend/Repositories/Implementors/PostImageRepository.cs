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

        public PostImage? GetImagesById(int imageId)
        {
            return _context.PostImages.FirstOrDefault(c => c.Id == imageId);
        }

        public ICollection<PostImage> GetImagesByPost(int postId)
        {
            return _context.PostImages.Where(c => c.PostId == postId).ToList();
        }

        public bool CreateImage(PostImage image)
        {
            _context.Add(image);
            return Save();
        }

        public bool UpdateImage(PostImage image)
        {
            _context.Update(image);
            return Save();
        }

        public bool DisableImage(PostImage image)
        {
            image.Status = false;
            _context.Remove(image);
            return Save();
        }

        public bool EnableImage(PostImage image)
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
