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
        /// <summary>
        /// Get list of Posts which are created and approved.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var existed = _postHandlers.GetAllPosts();
            if (existed != null) { return Ok(existed); }
            return NotFound();
        }

        /// <summary>
        /// Get list of Posts which are created and approved by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of Posts that are waiting for approval. (Moderator | Lecturer)
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of Posts belong to selected User that are waiting for approval. (Moderator | Lecturer)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("pending/{userId}")]
        public IActionResult ViewPendingPostListOf(int userId)
        {
            var existedList = _postHandlers.ViewPendingPostListOf(userId);
            if (existedList == null) return NotFound();
            return Ok(existedList);
        }

        /// <summary>
        /// Get list of deleted Posts of selected User. (Student | Moderator)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("deleted-post/{userId}")]
        public IActionResult ViewDeletedPostOf(int userId)
        {
            var existedList = _postHandlers.ViewDeletedPostOf(userId);
            if (existedList == null) return NotFound();
            return Ok(existedList);
        }

        /// <summary>
        /// Get list of Posts which are created and approved of selected User.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of Savelists by selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("save-lists/{postID}/{userID}")]
        public IActionResult GetSaveListByPostID(int postID, int userID)
        {
            var savelists = _postListHandlers.GetAllSaveListByPostID(postID, userID);
            if (savelists == null || savelists.Count == 0)
            {
                return NotFound();
            }
            return Ok(savelists);
        }

        /// <summary>
        /// Save the selected Post to the selected Savelist. (Student | Moderator)
        /// </summary>
        /// <param name="saveListID"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        [HttpPost("saved-post")]
        public IActionResult AddPostList(int saveListID, int postID)
        {
            var postList = _postListHandlers.AddPostList(saveListID, postID);
            if (postList == null)
            {
                return BadRequest();
            }
            return Ok(postList);
        }

        /// <summary>
        /// Create new Post. (Student | Moderator)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="tagIds"></param>
        /// <param name="categoryIds"></param>
        /// <param name="videoURLs"></param>
        /// <param name="imageURLs"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreatePost(int userId, 
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

        /// <summary>
        /// Update selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="tagIds"></param>
        /// <param name="categoryIds"></param>
        /// <param name="videoURLs"></param>
        /// <param name="imageURLs"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdatePost(int postId, 
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

        /// <summary>
        /// Ban/Disable/Delete selected Post. (Admin | Moderator | Student)
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Approve selected Post. (Moderator | Lecturer)
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        public IActionResult ApprovePost(int reviewerId, int postId)
        {
            var approvedPost = _postHandlers.ApprovePost(reviewerId, postId);
            if(approvedPost != null)
            {
                return Ok(approvedPost);
            }
            return BadRequest();
        }

        /// <summary>
        /// Deny selected Post. (Moderator | Lecturer)
        /// </summary>
        /// <param name="reviewerId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPut("deny")]
        public IActionResult DenyPost(int reviewerId, int postId)
        {
            var deniedPost = _postHandlers.DenyPost(reviewerId, postId);
            if(deniedPost != null)
            {
                return Ok(deniedPost);
            }
            return BadRequest();
        }

        [HttpGet("category-tag")]
        public IActionResult GetPostByCategoryAndTag([FromQuery] int[] categoryID, [FromQuery] int[] tagID, [FromQuery]string? searchValue)
        {
            var postList = _postHandlers.GetAllPosts(categoryID, tagID, searchValue);
            if(postList == null || postList.Count == 0)
            {
                return NotFound();
            }
            return Ok(postList);
        }
    }
}
