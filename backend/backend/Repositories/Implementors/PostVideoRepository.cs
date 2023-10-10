using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class PostVideoRepository : IPostVideoRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostVideoRepository(FBlogAcademyContext context)
        {
            _context = new();
        }

        public Video? GetVideoById(int videoId)
        {
            return _context.Videos.FirstOrDefault(c => c.Id == videoId);
        }

        public ICollection<PostVideo> GetVideoByPost(int postId)
        {
            return _context.PostVideos.Where(c => c.PostId == postId).ToList();
        }

        public bool CreateVideo(PostVideo video)
        {
            _context.Add(video);
            return Save();
        }

        public bool UpdateVideo(PostVideo video)
        {
            _context.Update(video);
            return Save();
        }

        public bool EnableVideo(PostVideo video)
        {
            video.Status = true;
            _context.Update(video);
            return Save();
        }

        public bool DisableVideo(PostVideo video)
        {
            video.Status = false;
            _context.Remove(video);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
