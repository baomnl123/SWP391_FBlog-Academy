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
        private readonly IVideoRepository _videoRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public VideoHandlers(IVideoRepository videoRepository, IPostRepository postRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public VideoDTO? GetVideoByID(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public VideoDTO? GetVideoByURL(string videoURL)
        {
            var video = _videoRepository.GetVideoByURL(videoURL);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public ICollection<VideoDTO>? GetVideosByPost(int postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            var videos = _videoRepository.GetVideosByPost(postId);
            return _mapper.Map<List<VideoDTO>>(videos);
        }

        public ICollection<VideoDTO>? CreateVideo(int postId, string[] videoURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            List<Video> videos = new();

            foreach (var url in videoURLs)
            {
                Video video = new()
                {
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then Add to list, else return null
                if (!_videoRepository.CreateVideo(postId, video)) return null;

                videos.Add(video);
            }

            return _mapper.Map<List<VideoDTO>>(videos);
        }

        public VideoDTO? DisableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == true) return null;

            // If disable succeed then return video, else return null
            if (_videoRepository.DisableVideo(video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }

        public VideoDTO? EnableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == true) return null;

            // If enable succeed then return video, else return null
            if (_videoRepository.EnableVideo(video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }

        public VideoDTO? UpdateVideo(int postId, int currentVideoId, string newVideoURL)
        {
            // If video was disabled or not found, return false
            var video = _videoRepository.GetVideoById(currentVideoId);
            if (video == null || video.Status == false) return null;

            video.Url = newVideoURL;

            // If update succeed then return video, else return null
            if (_videoRepository.DisableVideo(video))
                return _mapper.Map<VideoDTO>(video);

            return null;
        }
    }
}
