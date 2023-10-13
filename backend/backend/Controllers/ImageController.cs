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

        [HttpGet("{postId}/images")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostImage>))]
        [ProducesResponseType(400)]
        public IActionResult GetImageByPost(int postId )
        {
            var images = _imageHandlers.GetImagesByPost(postId);
            if(images == null) return NotFound();

            return Ok(images);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateImage([FromBody] int postId, string[] imageURLs)
        {
            // For each imageURL in imageURLs, if imageURL already exists then return
            if (imageURLs.Any(imageURL => _imageHandlers.GetImageByURL(imageURL) != null))
            {
                return StatusCode(422, "Image aldready exists!");
            }

            if (!_imageHandlers.CreateImage(postId, imageURLs))
                return BadRequest(ModelState);

            return Ok("Successfully create!");
        }

        [HttpPut("update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateImage([FromBody] string newImageURL, int currentImageId)
        {
            if (_imageHandlers.GetImageByURL(newImageURL) != null)
            {
                //ModelState.AddModelError("", "Image aldready exists!");
                return StatusCode(422, "Image aldready exists!");
            }

            if (!_imageHandlers.UpdateImage(currentImageId, newImageURL))
                return NotFound();

            return Ok("Update successfully!");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteImage(int imageId)
        {
            if (!_imageHandlers.DisableImage(imageId))
                ModelState.AddModelError("", "Something went wrong deleting category");

            return Ok("Delete successfully!");
        }
    }
}
