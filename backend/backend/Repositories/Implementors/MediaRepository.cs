using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class MediaRepository : IMediaRepository
    {
        private readonly FBlogAcademyContext _context;

        public MediaRepository()
        {
            _context = new();
        }

        public Media? GetMediaById(int MediaId)
        {
            return _context.Medias.FirstOrDefault(c => c.Id == MediaId);
        }

        public Media? GetMediaByURL(string MediaURL)
        {
            return _context.Medias.FirstOrDefault(c => c.Url == MediaURL);
        }

        public ICollection<Media>? GetMediasByPost(int postId)
        {
            return _context.Medias.Where(e => e.PostId == postId && e.Status == true).ToList();
        }

        public bool CreateMedia(Media Media)
        {
            _context.Add(Media);
            return Save();
        }

        public bool UpdateMedia(int postId, Media Media)
        {
            _context.Update(Media);
            return Save();
        }

        public bool DisableMedia(Media Media)
        {
            Media.Status = false;
            _context.Remove(Media);
            return Save();
        }

        public bool EnableMedia(Media Media)
        {
            Media.Status = true;
            _context.Update(Media);
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