using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostVideoRepository : IPostVideoRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostVideoRepository()
        {
            _context = new();
        }

        public bool CreateVideo(PostVideo postVideo)
        {
            _context.Add(postVideo);
            return Save();
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
            try
            {
                var saved = _context.SaveChanges();
                // if saved > 0 then return true, else return false
                return saved > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}
