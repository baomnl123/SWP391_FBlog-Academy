using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostImageHandlers : IPostImageHandlers
    {
        private readonly IPostImageRepository _postImageRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostImageHandlers(IPostImageRepository postImageRepository, IPostRepository postRepository, IMapper mapper)
        {
            _postImageRepository = postImageRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public ICollection<PostImage> GetImagesByPost(int postId)
        {
            //var images = _postRepository.get;
            throw new NotImplementedException();
            
        }

        public bool CreateImage(int postId, string image)
        {
            //var post = _postRepository.GetPost();
            throw new NotImplementedException();

        }

        public bool DisableImage(int imageId)
        {
            var image = _postImageRepository.GetImagesById(imageId);
            if (image.Status == false) return false;

            return _postImageRepository.DisableImage(image);
        }

        public bool EnableImage(int imageId)
        {
            var image = _postImageRepository.GetImagesById(imageId);
            if (image.Status == false) return false;

            return _postImageRepository.EnableImage(image);
        }

        public bool UpdateImage(int currentImageId, string newImage)
        {
            throw new NotImplementedException();
        }
    }
}
