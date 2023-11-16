using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class MediaHandlers : IMediaHandlers
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public MediaHandlers(IMediaRepository mediaRepository, IPostRepository postRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public MediaDTO? GetMediaByID(int MediaId)
        {
            var Media = _mediaRepository.GetMediaById(MediaId);
            if (Media == null || Media.Status == false) return null;

            return _mapper.Map<MediaDTO>(Media);
        }

        public MediaDTO? GetMediaByURL(string MediaURL)
        {
            var Media = _mediaRepository.GetMediaByURL(MediaURL);
            if (Media == null || Media.Status == false) return null;

            return _mapper.Map<MediaDTO>(Media);
        }

        public ICollection<MediaDTO>? GetMediasByPost(int postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            var Medias = _mediaRepository.GetMediasByPost(postId);
            return _mapper.Map<List<MediaDTO>>(Medias);

        }

        public ICollection<MediaDTO>? CreateMedia(int postId, string[] MediaURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            List<Media> Medias = new();

            foreach (var url in MediaURLs)
            {
                Media Media = new()
                {
                    PostId = postId,
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then Add to list, else return null
                if (!_mediaRepository.CreateMedia(Media)) return null;

                Medias.Add(Media);
            }

            return _mapper.Map<List<MediaDTO>>(Medias);
        }

        public MediaDTO? DisableMedia(int MediaId)
        {
            var Media = _mediaRepository.GetMediaById(MediaId);
            if (Media == null || Media.Status == false) return null;

            // If disable succeed then return Media, else return null
            if (_mediaRepository.DisableMedia(Media))
                return _mapper.Map<MediaDTO>(Media);

            return null;
        }

        public MediaDTO? EnableMedia(int MediaId)
        {
            var Media = _mediaRepository.GetMediaById(MediaId);
            if (Media == null || Media.Status == true) return null;

            // If enable succeed then return Media, else return null
            if (_mediaRepository.EnableMedia(Media))
                return _mapper.Map<MediaDTO>(Media);

            return null;
        }

        public MediaDTO? UpdateMedia(int postId, int currentMediaId, string newMediaURL)
        {
            // If Media was disabled or not found, return false
            var Media = _mediaRepository.GetMediaById(currentMediaId);
            if (Media == null || Media.Status == false) return null;

            Media.Url = newMediaURL;

            // If update succeed then return Media, else return null
            if (_mediaRepository.UpdateMedia(postId, Media))
                return _mapper.Map<MediaDTO>(Media);

            return null;
        }
    }
}
