using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostVideoRepository
    {
        public ICollection<PostVideo> GetPostVideosByPostId(int postId);
        public ICollection<PostVideo> GetPostVideosByVideoId(int videoId);
        public bool CreatePostVideo(PostVideo postVideo);
        public bool UpdatePostVideo(PostVideo postVideo);
        public bool EnablePostVideo(PostVideo postVideo);
        public bool DisablePostVideo(PostVideo postVideo);
        public bool Save();
    }
}
