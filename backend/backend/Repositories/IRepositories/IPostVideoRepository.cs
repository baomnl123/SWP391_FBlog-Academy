using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostVideoRepository
    {
        public ICollection<PostVideo> GetPostVideoByPostId(int postId);
        public ICollection<PostVideo> GetPostVideoByVideoId(int videoId);
        public bool CreateVideo(PostVideo postVideo);
        public bool EnableVideo(PostVideo postVideo);
        public bool DisableVideo(PostVideo postVideo);
        public bool Save();
    }
}
