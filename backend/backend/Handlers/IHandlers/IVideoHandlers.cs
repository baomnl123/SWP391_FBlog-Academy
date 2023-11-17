using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IVideoHandlers
    {
        public VideoDTO? GetVideoByID(int videoId);
        public VideoDTO? GetVideoByURL(string videoURL);
        public ICollection<VideoDTO>? GetVideosByPost(int postId);
        public ICollection<VideoDTO>? CreateVideo(int postId, string[] videoURLs);
        public VideoDTO? UpdateVideo(int postId, int currentVideoId, string newVideoURL);
        public VideoDTO? EnableVideo(int videoId);
        public VideoDTO? DisableVideo(int videoId);
    }
}