using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        private readonly IVideoHandlers _videoHandlers;

        public VideoController(IVideoHandlers videoHandlers)
        {
            _videoHandlers = videoHandlers;
        }

        [HttpGet("{postId}/videos")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostVideo>))]
        [ProducesResponseType(400)]
        public IActionResult GetVideoByPost(int postId)
        {
            var videos = _videoHandlers.GetVideosByPost(postId);
            if (videos == null) return NotFound();

            return Ok(videos);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateVideo([FromRoute] int postId, [FromBody] string[] videoURLs)
        {
            // For each videoURL in videoURLs, if videoURL already exists then return
            if (videoURLs.Any(videoURL => _videoHandlers.GetVideoByURL(videoURL) != null))
                return StatusCode(422, "Video aldready exists!");

            if (!_videoHandlers.CreateVideo(postId, videoURLs))
                return BadRequest(ModelState);

            return Ok("Successfully create!");
        }

        [HttpPut("update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVideo([FromQuery] int postId, [FromBody] string newVideoURL, int currentVideoId)
        {
            if (_videoHandlers.GetVideoByURL(newVideoURL) != null)
                return StatusCode(422, "Video aldready exists!");

            if (!_videoHandlers.UpdateVideo(postId, currentVideoId, newVideoURL))
                return NotFound();

            return Ok("Update successfully!");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteVideo(int videoId)
        {
            if (_videoHandlers.GetVideoByID(videoId) == null)
                return StatusCode(422, "Image aldready exists!");

            if (!_videoHandlers.DisableVideo(videoId))
                return BadRequest();

            return Ok("Delete successfully!");
        }
    }
}
