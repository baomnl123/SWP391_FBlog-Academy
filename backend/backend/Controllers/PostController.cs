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
        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var existed = _postHandlers.GetAllPosts();
            if (existed != null) { return Ok(existed); }
            return NotFound();
        }

        [HttpGet("{title}")]
        public IActionResult SearchPostsByTitle(string title)
        {
            var existed = _postHandlers.SearchPostsByTitle(title);

            if(existed != null)
            {
                return Ok(existed.ToList());
            }
            return NotFound();
        }

        [HttpGet("pending")]
        public IActionResult ViewPendingPostList()
        {
            var existed = _postHandlers.ViewPendingPostList();
            if (existed != null)
            {
                return Ok(existed.ToList());
            }
            return NotFound();
        }

        [HttpGet("{userId}")]
        public IActionResult SearchPostByUserId(int userId)
        {
            var existedPostList = _postHandlers.SearchPostByUserId(userId);
            if (existedPostList != null)
            {
                return Ok(existedPostList.ToList());
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreatePost([FromForm] int userId, 
                                        [FromForm] string title, 
                                        [FromForm] string content, 
                                        [FromForm] int[] tagIds, 
                                        [FromForm] int[] categoryIds,
                                        [FromForm] string[] videoURLs,
                                        [FromForm] string[] imageURLs)
        {
            var newPost = _postHandlers.CreatePost(userId, title, content, tagIds, categoryIds, videoURLs, imageURLs);
            if (newPost != null)
            {
                return Ok(newPost);
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult UpdatePost([FromForm] int postId, 
                                        [FromForm] string title, 
                                        [FromForm] string content, 
                                        [FromForm] int[] tagIds, 
                                        [FromForm] int[] categoryIds,
                                        [FromForm] string[] videoURLs,
                                        [FromForm] string[] imageURLs)
        {
            var updatedPost = _postHandlers.UpdatePost(postId, title, content, tagIds, categoryIds, videoURLs, imageURLs);
            if (updatedPost != null)
            {
                return Ok(updatedPost);
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeletePost([FromForm] int postId, 
                                        [FromForm] int tagId, 
                                        [FromForm] int categoryId)
        {
            var deletedPost = _postHandlers.DeletePost(postId);
            if(deletedPost != null)
            {
                return Ok(deletedPost);
            }
            return BadRequest();
        }

        [HttpPut("approve")]
        public IActionResult ApprovePost([FromForm] int reviewerId, [FromForm] int postId)
        {
            var approvedPost = _postHandlers.ApprovePost(reviewerId, postId);
            if(approvedPost != null)
            {
                return Ok(approvedPost);
            }
            return BadRequest();
        }

        [HttpPut("deny")]
        public IActionResult DenyPost([FromForm] int reviewerId, 
                                      [FromForm] int postId, 
                                      [FromForm] int tagId, 
                                      [FromForm] int categoryId)
        {
            var deniedPost = _postHandlers.DenyPost(reviewerId, postId, tagId, categoryId);
            if(deniedPost != null)
            {
                return Ok(deniedPost);
            }
            return BadRequest();
        }
    }
}
