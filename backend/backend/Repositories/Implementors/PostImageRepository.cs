using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class PostImageRepository : IPostImageRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostImageRepository(FBlogAcademyContext context)
        {
            _context = context;
        }

        public bool CreateImage(PostImage image)
        {
            _context.Add(image);
            return Save();
        }

        public bool DeleteImage(PostImage image)
        {
            _context.Remove(image);
            return Save();
        }

        public ICollection<PostImage> GetImagesByPost(int postId)
        {
            return _context.PostImages.Where(c => c.PostId == postId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateImage(PostImage image)
        {
            throw new NotImplementedException();
        }
    }
}
