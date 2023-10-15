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

        public bool CreateVideo(int postId, string[] videoURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return false;

            foreach (var url in videoURLs)
            {
                Video video = new()
                {
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then return true, else return false
                return _videoRepository.CreateVideo(postId, video);
            }

            return false;
        }

        public bool DisableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == true) return false;

            return _videoRepository.DisableVideo(video);
        }

        public bool EnableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == true) return false;

            return _videoRepository.EnableVideo(video);
        }

        public bool UpdateVideo(int postId, int currentVideoId, string newVideoURL)
        {
            // If video was disabled or not found, return false
            var video = _videoRepository.GetVideoById(currentVideoId);
            if (video == null || video.Status == false) return false;

            // If update succeed then return true, else return false
            video.Url = newVideoURL;
            return _videoRepository.UpdateVideo(postId, video);
        }
    }
}
