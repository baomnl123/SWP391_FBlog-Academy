using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.Implementors;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class ImageHandlers : IImageHandlers
    {
        private readonly IImageRepository _imageRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public ImageHandlers(IImageRepository imageRepository, IPostRepository postRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public ImageDTO? GetImageByID(int imageId)
        {
            var image = _imageRepository.GetImageById(imageId);
            if (image == null || image.Status == false) return null;

            return _mapper.Map<ImageDTO>(image);
        }

        public ImageDTO? GetImageByURL(string imageURL)
        {
            var image = _imageRepository.GetImageByURL(imageURL);
            if (image == null || image.Status == false) return null;

            return _mapper.Map<ImageDTO>(image);
        }

        public ICollection<ImageDTO>? GetImagesByPost(int postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            var images = _imageRepository.GetImagesByPost(postId);
            return _mapper.Map<List<ImageDTO>>(images);

        }

        public bool CreateImage(int postId, string[] imageURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return false;

            foreach (var url in imageURLs)
            {
                Image image = new()
                {
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then return true, else return false
                return _imageRepository.CreateImage(postId, image);
            }

            return false;
        }

        public bool DisableImage(int imageId)
        {
            var image = _imageRepository.GetImageById(imageId);
            if (image == null || image.Status == false) return true;

            return _imageRepository.DisableImage(image);
        }

        public bool EnableImage(int imageId)
        {
            var image = _imageRepository.GetImageById(imageId);
            if (image == null || image.Status == true) return true;

            return _imageRepository.EnableImage(image);
        }

        public bool UpdateImage(int postId, int currentImageId, string newImageURL)
        {
            // If image was disabled or not found, return false
            var image = _imageRepository.GetImageById(currentImageId);
            if (image == null || image.Status == false) return false;

            // If update succeed then return true, else return false
            image.Url = newImageURL;
            return _imageRepository.UpdateImage(postId, image);
        }
    }
}
