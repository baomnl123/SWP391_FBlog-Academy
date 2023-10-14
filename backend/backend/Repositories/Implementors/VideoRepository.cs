using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class VideoRepository : IVideoRepository
    {
        private readonly FBlogAcademyContext _context;

        public VideoRepository()
        {
            _context = new();
        }

        public Video? GetVideoById(int videoId)
        {
            return _context.Videos.FirstOrDefault(c => c.Id == videoId);
        }

        public Video? GetVideoByURL(string videoURL)
        {
            return _context.Videos.FirstOrDefault(c => c.Url == videoURL);
        }

        public ICollection<Video> GetVideosByPost(int postId)
        {
            return _context.PostVideos.Where(e => e.PostId == postId && e.Status == true)
                                      .Select(e => e.Video)
                                      .Where(c => c.Status == true).ToList();
        }

        public bool CreateVideo(Video video)
        {
            _context.Add(video);
            return Save();
        }

        public bool UpdateVideo(Video video)
        {
            _context.Update(video);
            return Save();
        }

        public bool EnableVideo(Video video)
        {
            video.Status = true;
            _context.Update(video);
            return Save();
        }

        public bool DisableVideo(Video video)
        {
            video.Status = false;
            _context.Remove(video);
            return Save();
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
