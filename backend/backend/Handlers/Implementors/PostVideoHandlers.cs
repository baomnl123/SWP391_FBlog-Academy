using AutoMapper;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostVideoHandlers : IPostVideoHandlers
    {
        private readonly IPostVideoRepository _postVideoRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostVideoHandlers(IPostVideoRepository postVideoRepository, IPostRepository postRepository, IMapper mapper)
        {
            _postVideoRepository = postVideoRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public ICollection<PostVideo> GetVideoByPost(int postId)
        {
            throw new NotImplementedException();
        }

        public bool CreateVideo(int postId, string video)
        {
            throw new NotImplementedException();
        }

        public bool DisableVideo(int videoId)
        {
            throw new NotImplementedException();
        }

        public bool EnableVideo(int videoId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateVideo(int currentVideoId, string newVideo)
        {
            throw new NotImplementedException();
        }
    }
}
