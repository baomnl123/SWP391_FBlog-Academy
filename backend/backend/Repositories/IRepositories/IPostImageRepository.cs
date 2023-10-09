using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public PostImage? GetImagesById(int imageId);
        public ICollection<PostImage> GetImagesByPost(int postId);
        public bool CreateImage(PostImage image);
        public bool UpdateImage(PostImage image);
        public bool EnableImage(PostImage image);
        public bool DisableImage(PostImage image);
        public bool Save();
    }
}
