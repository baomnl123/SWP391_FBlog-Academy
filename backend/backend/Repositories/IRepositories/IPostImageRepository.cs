using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public ICollection<PostImage> GetImagesByPost(int postId);
        public bool CreateImage(PostImage image);
        public bool UpdateImage(PostImage image);
        public bool DeleteImage(PostImage image);
        public bool Save();
    }
}
