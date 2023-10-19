using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public ICollection<PostImage> GetPostImagesByPostId(int postId);
        public ICollection<PostImage> GetPostImagesByImageId(int imageId);
        public bool CreatePostImage(PostImage postImage);
        public bool UpdatePostImage(PostImage postImage);
        public bool EnablePostImage(PostImage postImage);
        public bool DisablePostImage(PostImage postImage);
        public bool Save();
    }
}
