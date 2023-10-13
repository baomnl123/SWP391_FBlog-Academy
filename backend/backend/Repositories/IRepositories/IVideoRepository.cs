using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IVideoRepository
    {
        public Video? GetVideoById(int videoId);
        public Video? GetVideoByURL(string videoURL);
        public ICollection<Video> GetVideosByPost(int postId);
        public bool CreateVideo(Video video);
        public bool UpdateVideo(Video video);
        public bool EnableVideo(Video video);
        public bool DisableVideo(Video video);
        public bool Save();
    }
}
