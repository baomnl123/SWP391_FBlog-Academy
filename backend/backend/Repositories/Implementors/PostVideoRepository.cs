using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class PostVideoRepository : IPostVideoRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostVideoRepository()
        {
            _context = new();
        }

        public bool DisableVideo(PostVideo postVideo)
        {
            postVideo.Status = false;
            _context.Update(postVideo);
            return Save();
        }

        public bool EnableVideo(PostVideo postVideo)
        {
            postVideo.Status = true;
            _context.Update(postVideo);
            return Save();
        }

        public ICollection<PostVideo> GetPostVideoByPostId(int postId)
        {
            return _context.PostVideos.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<PostVideo> GetPostVideoByVideoId(int videoId)
        {
            return _context.PostVideos.Where(c => c.VideoId == videoId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
