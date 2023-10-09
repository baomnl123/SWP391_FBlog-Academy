using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostVideoRepository
    {
        public ICollection<User> GetVideoByUser(int userId);
        public ICollection<Post> GetVideoByPost(int postId);
        public bool CreateVideo(PostVideo video);
        public bool UpdateVideo(PostVideo video);
        public bool DeleteVideo(PostVideo video);
        public bool Save();
    }
}
