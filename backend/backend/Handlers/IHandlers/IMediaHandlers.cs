using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IMediaHandlers
    {
        public MediaDTO? GetMediaByID(int MediaId);
        public MediaDTO? GetMediaByURL(string MediaURL);
        public ICollection<MediaDTO>? GetMediasByPost(int postId);
        public ICollection<MediaDTO>? CreateMedia(int postId, string[] MediaURLs);
        public MediaDTO? UpdateMedia(int postId, int currentMediaId, string newMediaURL);
        public MediaDTO? EnableMedia(int MediaId);
        public MediaDTO? DisableMedia(int MediaId);
    }
}
