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

        public ICollection<ImageDTO>? CreateImage(int postId, string[] imageURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            List<Image> images = new();

            foreach (var url in imageURLs)
            {
                Image image = new()
                {
                    PostId = postId,
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then Add to list, else return null
                if (!_imageRepository.CreateImage(image)) return null;

                images.Add(image);
            }

            return _mapper.Map<List<ImageDTO>>(images);
        }

        public ImageDTO? DisableImage(int imageId)
        {
            var image = _imageRepository.GetImageById(imageId);
            if (image == null || image.Status == false) return null;

            // If disable succeed then return image, else return null
            if (_imageRepository.DisableImage(image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }

        public ImageDTO? EnableImage(int imageId)
        {
            var image = _imageRepository.GetImageById(imageId);
            if (image == null || image.Status == true) return null;

            // If enable succeed then return image, else return null
            if (_imageRepository.EnableImage(image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }

        public ImageDTO? UpdateImage(int postId, int currentImageId, string newImageURL)
        {
            // If image was disabled or not found, return false
            var image = _imageRepository.GetImageById(currentImageId);
            if (image == null || image.Status == false) return null;

            image.Url = newImageURL;

            // If update succeed then return image, else return null
            if (_imageRepository.UpdateImage(postId, image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }
    }
}
