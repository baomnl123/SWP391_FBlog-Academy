using AutoMapper;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageHandlers _imageHandlers;

        public ImageController(IImageHandlers imageHandlers)
        {
            _imageHandlers = imageHandlers;
        }

        /// <summary>
        /// Get all Images by selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}/images")]
        [ProducesResponseType(400)]
        public IActionResult GetImageByPost(int postId)
        {
            var images = _imageHandlers.GetImagesByPost(postId);
            if (images == null) return NotFound();

            return Ok(images);
        }

        /// <summary>
        /// Create new Images for selected Post (Student | Moderator)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="imageURLs"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateImage([FromForm] int postId, [FromForm] string[] imageURLs)
        {
            // For each imageURL in imageURLs, if imageURL already exists then return
            if (imageURLs.Any(imageURL => _imageHandlers.GetImageByURL(imageURL) != null))
                return StatusCode(422, "Image aldready exists!");

            var createImage = _imageHandlers.CreateImage(postId, imageURLs);
            if (createImage == null) return BadRequest(ModelState);

            return Ok(createImage);
        }

        /// <summary>
        /// Update Image for selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="newImageURL"></param>
        /// <param name="currentImageId"></param>
        /// <returns></returns>
        [HttpPut("{postId}/{currentImageId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateImage(int postId, [FromForm] string newImageURL, int currentImageId)
        {
            if (_imageHandlers.GetImageByURL(newImageURL) != null)
                return StatusCode(422, "Image aldready exists!");
            
            var updateImage = _imageHandlers.UpdateImage(postId, currentImageId, newImageURL);
            if (updateImage == null) return BadRequest();

            return Ok(updateImage);
        }

        /// <summary>
        /// Delete Image of selected Post. (Student | Moderator | Admin)
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpDelete("{imageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteImage(int imageId)
        {
            if (_imageHandlers.GetImageByID(imageId) == null)
                return StatusCode(422, "Image aldready exists!");

            var deleteImage = _imageHandlers.DisableImage(imageId);
            if (deleteImage == null) return BadRequest();

            return Ok("Delete successfully!");
        }
    }
}
