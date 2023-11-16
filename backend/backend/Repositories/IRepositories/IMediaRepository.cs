using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IMediaRepository
    {
        public Media? GetMediaById(int MediaId);
        public Media? GetMediaByURL(string MediaURL);
        public ICollection<Media>? GetMediasByPost(int postId);
        public bool CreateMedia(Media Media);
        public bool UpdateMedia(int postId, Media Media);
        public bool EnableMedia(Media Media);
        public bool DisableMedia(Media Media);
        public bool Save();
    }
}
