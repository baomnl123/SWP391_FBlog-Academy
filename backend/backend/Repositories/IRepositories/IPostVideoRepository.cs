using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostVideoRepository
    {
        //public ICollection<User> GetVideoByUser(int userId);
        public PostVideo? GetVideoById(int videoId);
        public ICollection<PostVideo> GetVideoByPost(int postId);
        public bool CreateVideo(PostVideo video);
        public bool UpdateVideo(PostVideo video);
        public bool EnableVideo(PostVideo video);
        public bool DisableVideo(PostVideo video);
        public bool Save();
    }
}
