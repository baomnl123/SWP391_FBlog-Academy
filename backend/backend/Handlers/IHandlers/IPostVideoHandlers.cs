using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IPostVideoHandlers
    {
        public ICollection<PostVideo> GetVideoByPost(int postId);
        public bool CreateVideo(int postId, string video);
        public bool UpdateVideo(int currentVideoId, string newVideo);
        public bool EnableVideo(int videoId);
        public bool DisableVideo(int videoId);
    }
}
