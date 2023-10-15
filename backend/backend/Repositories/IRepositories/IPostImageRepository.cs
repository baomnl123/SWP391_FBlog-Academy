using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public ICollection<PostImage> GetPostImageByPostId(int postId);
        public ICollection<PostImage> GetPostImageByImageId(int imageId);
        public bool CreateImage(PostImage postImage);
        public bool EnableImage(PostImage postImage);
        public bool DisableImage(PostImage postImage);
        public bool Save();
    }
}
