using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostHandlers _postHandlers;
        private readonly IPostListHandlers _postListHandlers;

        public PostController(IPostHandlers postHandlers, IPostListHandlers postListHandlers)
        {
            _postHandlers = postHandlers;
            _postListHandlers = postListHandlers;
        }
        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var existed = _postHandlers.GetAllPosts();
            if (existed != null) { return Ok(existed); }
            return NotFound();
        }

        [HttpGet("title/{title}")]
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

        [HttpGet("pending/{userId}")]
        public IActionResult ViewPendingPostListOf(int userId)
        {
            var existedList = _postHandlers.ViewPendingPostListOf(userId);
            if (existedList == null) return NotFound();
            return Ok(existedList);
        }

        [HttpGet("deleted-post/{userId}")]
        public IActionResult ViewDeletedPostOf(int userId)
        {
            var existedList = _postHandlers.ViewDeletedPostOf(userId);
            if (existedList == null) return NotFound();
            return Ok(existedList);
        }

        [HttpGet("user/{userId}")]
        public IActionResult SearchPostByUserId(int userId)
        {
            var existedPostList = _postHandlers.SearchPostByUserId(userId);
            if (existedPostList != null)
            {
                return Ok(existedPostList.ToList());
            }
            return NotFound();
        }

        [HttpGet("saved-lists")]
        public IActionResult GetSaveListByPostID([FromForm] int postID, [FromForm] int userID)
        {
            var savelists = _postListHandlers.GetAllSaveListByPostID(postID, userID);
            if (savelists == null || savelists.Count == 0)
            {
                return NotFound();
            }
            return Ok(savelists);
        }

        [HttpPost("saved-post")]
        public IActionResult AddPostList([FromForm] int saveListID, [FromForm] int postID)
        {
            var postList = _postListHandlers.AddPostList(saveListID, postID);
            if (postList == null)
            {
                return BadRequest();
            }
            return Ok(postList);
        }

        [HttpPost]
        public IActionResult CreatePost([FromForm] int userId, 
                                        [FromForm] string title, 
                                        [FromForm] string content, 
                                        [FromForm] int[]? tagIds, 
                                        [FromForm] int[]? categoryIds,
                                        [FromForm] string[]? videoURLs,
                                        [FromForm] string[]? imageURLs)
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

        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            var deletedPost = _postHandlers.DisablePost(postId);
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
                                      [FromForm] int postId)
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
