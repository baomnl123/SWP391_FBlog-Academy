using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IImageHandlers
    {
        public ImageDTO? GetImageByID(int imageId);
        public ImageDTO? GetImageByURL(string imageURL);
        public ICollection<ImageDTO>? GetImagesByPost(int postId);
        public bool CreateImage(int postId, string[] imageURLs);
        public bool UpdateImage(int postId, int currentImageId, string newImageURL);
        public bool EnableImage(int imageId);
        public bool DisableImage(int imageId);
    }
}
