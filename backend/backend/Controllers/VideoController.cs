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
        [ProducesResponseType(400)]
        public IActionResult GetVideoByPost(int postId)
        {
            var videos = _videoHandlers.GetVideosByPost(postId);
            if (videos == null) return NotFound();

            return Ok(videos);
        }

        [HttpPost("create-video")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateVideo([FromForm] int postId, [FromForm] string[] videoURLs)
        {
            // For each videoURL in videoURLs, if videoURL already exists then return
            if (videoURLs.Any(videoURL => _videoHandlers.GetVideoByURL(videoURL) != null))
                return StatusCode(422, "Video aldready exists!");

            var createVideo = _videoHandlers.CreateVideo(postId, videoURLs);
            if (createVideo == null) return BadRequest(ModelState);

            return Ok(createVideo);
        }

        [HttpPut("update/{postId}/{currentVideoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVideo(int postId, [FromForm] string newVideoURL, int currentVideoId)
        {
            if (_videoHandlers.GetVideoByURL(newVideoURL) != null)
                return StatusCode(422, "Video aldready exists!");

            var updateVideo = _videoHandlers.UpdateVideo(postId, currentVideoId, newVideoURL);
            if (updateVideo == null) return NotFound();

            return Ok(updateVideo);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteVideo(int videoId)
        {
            if (_videoHandlers.GetVideoByID(videoId) == null)
                return StatusCode(422, "Image aldready exists!");

            var deleteVideo = _videoHandlers.DisableVideo(videoId);
            if (deleteVideo == null) return BadRequest();

            return Ok("Delete successfully!");
        }
    }
}
