using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostImageRepository
    {
        public ICollection<User> GetImageByUser(int userId);
        public ICollection<Post> GetImageByPost(int postId);
        public bool CreateImage(PostImage image);
        public bool UpdateImage(PostImage image);
        public bool DeleteImage(PostImage image);
        public bool Save();
    }
}
