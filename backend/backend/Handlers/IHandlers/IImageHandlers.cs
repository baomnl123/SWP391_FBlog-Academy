using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IImageHandlers
    {
        public ImageDTO? GetImageByID(int imageId);
        public ImageDTO? GetImageByURL(string imageURL);
        public ICollection<ImageDTO>? GetImagesByPost(int postId);
        public ICollection<ImageDTO>? CreateImage(int postId, string[] imageURLs);
        public ImageDTO? UpdateImage(int postId, int currentImageId, string newImageURL);
        public ImageDTO? EnableImage(int imageId);
        public ImageDTO? DisableImage(int imageId);
    }
}
