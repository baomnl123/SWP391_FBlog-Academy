using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IPostImageHandlers
    {
        public ICollection<PostImage> GetImagesByPost(int postId);
        public bool CreateImage(int postId, string image);
        public bool UpdateImage(int currentImageId, string newImage);
        public bool EnableImage(int imageId);
        public bool DisableImage(int imageId);
    }
}
