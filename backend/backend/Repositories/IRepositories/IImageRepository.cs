using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IImageRepository
    {
        public Image? GetImageById(int imageId);
        public Image? GetImageByURL(string imageURL);
        public ICollection<Image>? GetImagesByPost(int postId);
        public bool CreateImage(Image image);
        public bool UpdateImage(int postId, Image image);
        public bool EnableImage(Image image);
        public bool DisableImage(Image image);
        public bool Save();
    }
}
