using backend.Models;
using backend.Repositories.IRepositories;

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
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
