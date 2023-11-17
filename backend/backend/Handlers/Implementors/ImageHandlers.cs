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
        private readonly IMediaRepository _mediaRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public ImageHandlers(IMediaRepository mediaRepository, IPostRepository postRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public ImageDTO? GetImageByID(int imageId)
        {
            var image = _mediaRepository.GetMediaById(imageId);
            if (image == null || image.Status == false) return null;

            return _mapper.Map<ImageDTO>(image);
        }

        public ImageDTO? GetImageByURL(string imageURL)
        {
            var image = _mediaRepository.GetMediaByURL(imageURL);
            if (image == null || image.Status == false) return null;

            return _mapper.Map<ImageDTO>(image);
        }

        public ICollection<ImageDTO>? GetImagesByPost(int postId)
        {
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            var images = _mediaRepository.GetMediasByPost(postId)?.Where(x => x.Type == "image").ToList();

            return _mapper.Map<List<ImageDTO>>(images);

        }

        public ICollection<ImageDTO>? CreateImage(int postId, string[] imageURLs)
        {
            // Find post
            var post = _postRepository.GetPost(postId);
            if (post == null || post.Status == false) return null;

            List<Media> images = new();

            foreach (var url in imageURLs)
            {
                Media image = new()
                {
                    PostId = postId,
                    Type = "image",
                    Url = url,
                    CreatedAt = DateTime.Now,
                    Status = true
                };
                // If create succeed then Add to list, else return null
                if (!_mediaRepository.CreateMedia(image)) return null;

                images.Add(image);
            }

            return _mapper.Map<List<ImageDTO>>(images);
        }

        public ImageDTO? DisableImage(int imageId)
        {
            var image = _mediaRepository.GetMediaById(imageId);
            if (image == null || image.Status == false) return null;

            // If disable succeed then return image, else return null
            if (_mediaRepository.DisableMedia(image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }

        public ImageDTO? EnableImage(int imageId)
        {
            var image = _mediaRepository.GetMediaById(imageId);
            if (image == null || image.Status == true) return null;

            // If enable succeed then return image, else return null
            if (_mediaRepository.EnableMedia(image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }

        public ImageDTO? UpdateImage(int postId, int currentImageId, string newImageURL)
        {
            // If image was disabled or not found, return false
            var image = _mediaRepository.GetMediaById(currentImageId);
            if (image == null || image.Status == false) return null;

            image.Url = newImageURL;

            // If update succeed then return image, else return null
            if (_mediaRepository.UpdateMedia(postId, image))
                return _mapper.Map<ImageDTO>(image);

            return null;
        }
    }
}