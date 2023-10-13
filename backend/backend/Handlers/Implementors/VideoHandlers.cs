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
        private readonly IPostVideoRepository _postVideoRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public VideoHandlers(IVideoRepository videoRepository, 
                             IPostVideoRepository postVideoRepository, 
                             IPostRepository postRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _postVideoRepository = postVideoRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public VideoDTO GetVideoByID(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public VideoDTO GetVideoByURL(string videoURL)
        {
            var video = _videoRepository.GetVideoByURL(videoURL);
            if (video == null || video.Status == false) return null;

            return _mapper.Map<VideoDTO>(video);
        }

        public ICollection<VideoDTO> GetVideoByPost(int postId)
        {
            //var post = _postRepository.GetPostById(postId);
            //if (post == null || post.Status == false) return null;

            var videos = _videoRepository.GetVideosByPost(postId);
            return _mapper.Map<List<VideoDTO>>(videos);
        }

        public bool CreateVideo(int postId, string[] videos)
        {
            throw new NotImplementedException();
        }

        public bool DisableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video.Status == false) return false;

            return _videoRepository.DisableVideo(video);
        }

        public bool EnableVideo(int videoId)
        {
            var video = _videoRepository.GetVideoById(videoId);
            if (video.Status == false) return false;

            return _videoRepository.EnableVideo(video);
        }

        public bool UpdateVideo(int currentVideoId, string newVideoURL)
        {
            // If video was disabled or not found, return false
            var video = _videoRepository.GetVideoById(currentVideoId);
            if (video == null || video.Status == false) return false;

            // If update succeed then return true, else return false
            video.Url = newVideoURL;
            return _videoRepository.UpdateVideo(video);
        }
    }
}
