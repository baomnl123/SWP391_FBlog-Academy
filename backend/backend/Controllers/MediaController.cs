using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : Controller
    {
        private readonly IMediaHandlers _MediaHandlers;

        public MediaController(IMediaHandlers MediaHandlers)
        {
            _MediaHandlers = MediaHandlers;
        }

        /// <summary>
        /// Get all Medias by selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}/Medias")]
        [ProducesResponseType(400)]
        public IActionResult GetMediaByPost(int postId)
        {
            var Medias = _MediaHandlers.GetMediasByPost(postId);
            if (Medias == null)
            {
                var emptyList = new List<MediaDTO>();
                return Ok(emptyList);
            }

            return Ok(Medias);
        }

        /// <summary>
        /// Create new Medias for selected Post (Student | Moderator)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="MediaURLs"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateMedia([FromForm] int postId, [FromForm] string[] MediaURLs)
        {
            // For each MediaURL in MediaURLs, if MediaURL already exists then return
            if (MediaURLs.Any(MediaURL => _MediaHandlers.GetMediaByURL(MediaURL) != null))
                return StatusCode(422, "Media aldready exists!");

            var createMedia = _MediaHandlers.CreateMedia(postId, MediaURLs);
            if (createMedia == null) return BadRequest(ModelState);

            return Ok(createMedia);
        }

        /// <summary>
        /// Update Media for selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="newMediaURL"></param>
        /// <param name="currentMediaId"></param>
        /// <returns></returns>
        [HttpPut("{postId}/{currentMediaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMedia(int postId, [FromForm] string newMediaURL, int currentMediaId)
        {
            if (_MediaHandlers.GetMediaByURL(newMediaURL) != null)
                return StatusCode(422, "Media aldready exists!");
            
            var updateMedia = _MediaHandlers.UpdateMedia(postId, currentMediaId, newMediaURL);
            if (updateMedia == null) return BadRequest();

            return Ok(updateMedia);
        }

        /// <summary>
        /// Delete Media of selected Post. (Student | Moderator | Admin)
        /// </summary>
        /// <param name="MediaId"></param>
        /// <returns></returns>
        [HttpDelete("{MediaId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMedia(int MediaId)
        {
            if (_MediaHandlers.GetMediaByID(MediaId) == null)
                return StatusCode(422, "Media aldready exists!");

            var deleteMedia = _MediaHandlers.DisableMedia(MediaId);
            if (deleteMedia == null) return BadRequest();

            return Ok("Delete successfully!");
        }
    }
}
