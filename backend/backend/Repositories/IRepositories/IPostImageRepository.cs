using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public Image? GetImagesById(int imageId);
        public ICollection<PostImage> GetImagesByPost(int postId);
        public bool CreateImage(Image image);
        public bool UpdateImage(Image image);
        public bool EnableImage(Image image);
        public bool DisableImage(Image image);
        public bool Save();
    }
}
