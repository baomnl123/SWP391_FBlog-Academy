using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class VideoHandlers : IVideoHandlers
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public VideoHandlers(IMediaRepository mediaRepository, IPostRepository postRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public VideoDTO? GetVideoByID(int videoId)
        {
            var video = _mediaRepository.GetMediaById(videoId);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public VideoDTO? GetVideoByURL(string videoURL)
        {
            var video = _mediaRepository.GetMediaByURL(videoURL);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public ICollection<VideoDTO>? GetVideosByPost(int postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            var videos = _mediaRepository.GetMediasByPost(postId)?.Where(x => x.Type == "video").ToList();
            return _mapper.Map<List<VideoDTO>>(videos);

        }

        public ICollection<VideoDTO>? CreateVideo(int postId, string[] videoURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            List<Media> videos = new();

            foreach (var url in videoURLs)
            {
                Media video = new()
                {
                    PostId = postId,
                    Type = "video",
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then Add to list, else return null
                if (!_mediaRepository.CreateMedia(video)) return null;

                videos.Add(video);
            }

            return _mapper.Map<List<VideoDTO>>(videos);
        }

        public VideoDTO? DisableVideo(int videoId)
        {
            var video = _mediaRepository.GetMediaById(videoId);
            if (video == null || video.Status == false) return null;

            // If disable succeed then return video, else return null
            if (_mediaRepository.DisableMedia(video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }

        public VideoDTO? EnableVideo(int videoId)
        {
            var video = _mediaRepository.GetMediaById(videoId);
            if (video == null || video.Status == true) return null;

            // If enable succeed then return video, else return null
            if (_mediaRepository.EnableMedia(video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }

        public VideoDTO? UpdateVideo(int postId, int currentVideoId, string newVideoURL)
        {
            // If video was disabled or not found, return false
            var video = _mediaRepository.GetMediaById(currentVideoId);
            if (video == null || video.Status == false) return null;

            video.Url = newVideoURL;

            // If update succeed then return video, else return null
            if (_mediaRepository.UpdateMedia(postId, video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }
    }
}