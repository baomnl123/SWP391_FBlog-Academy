using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostHandlers _postHandlers;

        public PostController(IPostHandlers postHandlers)
        {
            _postHandlers = postHandlers;
        }
        [HttpGet("SearchAllPosts")]
        public IActionResult GetAllPosts()
        {
            var existed = _postHandlers.GetAllPosts();
            if (existed != null) { return Ok(existed); }
            return NotFound();
        }

        [HttpGet("SearchPostsByTitle")]
        public IActionResult SearchPostsByTitle(string title)
        {
            var existed = _postHandlers.SearchPostsByTitle(title);

            if(existed != null)
            {
                return Ok(existed.ToList());
            }
            return NotFound();
        }

        [HttpGet("ViewPendingPostList")]
        public IActionResult ViewPendingPostList(int viewerId)
        {
            var existed = _postHandlers.ViewPendingPostList(viewerId);
            if (existed != null)
            {
                return Ok(existed.ToList());
            }
            return NotFound();
        }

        [HttpGet("SearchPostByUserId")]
        public IActionResult SearchPostByUserId(int userId)
        {
            var existedPostList = _postHandlers.SearchPostByUserId(userId);
            if (existedPostList != null)
            {
                return Ok(existedPostList.ToList());
            }
            return NotFound();
        }

        [HttpPost("CreateNewPost")]
        public IActionResult CreatePost(int userId, string title, string content)
        {
            var newPost = _postHandlers.CreatePost(userId, title, content);
            if (newPost != null)
            {
                return Ok(newPost);
            }
            return BadRequest();
        }

        [HttpPut("UpdatePost")]
        public IActionResult UpdatePost(int postId, string title,string content)
        {
            var updatedPost = _postHandlers.UpdatePost(postId, title, content);
            if (updatedPost != null)
            {
                return Ok(updatedPost);
            }
            return BadRequest();
        }

        [HttpDelete("DeletePost")]
        public IActionResult DeletePost(int postId)
        {
            var deletedPost = _postHandlers.DeletePost(postId);
            if(deletedPost != null)
            {
                return Ok(deletedPost);
            }
            return BadRequest();
        }

        [HttpPut("ApprovePost")]
        public IActionResult ApprovePost(int reviewerId, int postId)
        {
            var approvedPost = _postHandlers.ApprovePost(reviewerId, postId);
            if(approvedPost != null)
            {
                return Ok(approvedPost);
            }
            return BadRequest();
        }

        [HttpPut("DenyPost")]
        public IActionResult DenyPost(int reviewerId, int postId)
        {
            var deniedPost = _postHandlers.DenyPost(reviewerId, postId);
            if(deniedPost != null)
            {
                return Ok(deniedPost);
            }
            return BadRequest();
        }
    }
}
