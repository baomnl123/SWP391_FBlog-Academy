using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Utils;
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
        private readonly EmailSender _emailSender;

        public PostController(IPostHandlers postHandlers, IPostListHandlers postListHandlers)
        {
            _postHandlers = postHandlers;
            _postListHandlers = postListHandlers;
            _emailSender = new EmailSender();
        }
        /// <summary>
        /// Get list of Posts which are created and approved.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/{currentUserId}")]
        public IActionResult GetAllPosts(int currentUserId)
        {
            var existed = _postHandlers.GetAllPostsForAdmin(currentUserId);
            if (existed == null || existed.Count == 0)
            {
                existed = new List<PostDTO>();
            }
            return Ok(existed);
        }

        /// <summary>
        /// Get list of Posts which are created and approved by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("title")]
        public IActionResult SearchPostsByTitle(string title, int currentUserId)
        {
            var existed = _postHandlers.SearchPostsByTitle(title, currentUserId);

            if (existed == null || existed.Count == 0)
            {
                existed = new List<PostDTO>();
            }
            return Ok(existed.ToList());
        }

        /// <summary>
        /// Get list of Posts that are waiting for approval. (Moderator | Lecturer)
        /// </summary>
        /// <returns></returns>
        [HttpGet("pending")]
        public IActionResult ViewPendingPostList(int currentUserId)
        {
            var existed = _postHandlers.ViewPendingPostList(currentUserId);
            if (existed == null || existed.Count == 0)
            {
                existed = new List<PostDTO>();
            }
            return Ok(existed.ToList());
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
            if (existedList == null || existedList.Count == 0)
            {
                existedList = new List<PostDTO>();
            }
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
            if (existedList == null || existedList.Count == 0)
            {
                existedList = new List<PostDTO>();
            }
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
            if (existedPostList == null || existedPostList.Count == 0)
            {
                existedPostList = new List<PostDTO>();
            }
            return Ok(existedPostList.ToList());
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
                savelists = new List<SaveListDTO>();
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
        /// <param name="subjectIds"></param>
        /// <param name="majorIds"></param>
        /// <param name="MediaURLs"></param>
        /// <param name="mediaURLs"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreatePost(int userId,
                                        [FromForm] string title,
                                        [FromForm] string content,
                                        [FromQuery] int[]? subjectIds,
                                        [FromQuery] int[]? majorIds,
                                        [FromQuery] string[]? imageURLs,
                                        [FromQuery] string[]? videoURLs)
        {
            var newPost = _postHandlers.CreatePost(userId, title, content, subjectIds, majorIds, imageURLs, videoURLs);
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
        /// <param name="subjectIds"></param>
        /// <param name="majorIds"></param>
        /// <param name="imageURLs"></param>
        /// <param name="videoURLs"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdatePost(int postId,
                                        [FromForm] string title,
                                        [FromForm] string content,
                                        [FromQuery] int[] subjectIds,
                                        [FromQuery] int[] majorIds,
                                        [FromQuery] string[] imageURLs,
                                        [FromQuery] string[] videoURLs)
        {
            var updatedPost = _postHandlers.UpdatePost(postId, title, content, subjectIds, majorIds, imageURLs, videoURLs);
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
        public async Task<IActionResult> DeletePost(int postId)
        {
            var deletedPost = _postHandlers.DisablePost(postId);
            if (deletedPost != null)
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
        public async Task<IActionResult> ApprovePost(int reviewerId, int postId)
        {
            var approvedPost = _postHandlers.ApprovePost(reviewerId, postId);
            if (approvedPost != null)
            {
                //send email
                var existedEmail = approvedPost.User.Email;
                var existedSubject = $"Your post has been approved";
                var existedMessage = $"The \"{approvedPost.Title}\" post has been approved by the Reviewer.\nYour post will be displayed on the blog.\n\nFaithfully, FBLog";

                await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
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
        public async Task<IActionResult> DenyPost(int reviewerId, int postId)
        {
            var deniedPost = _postHandlers.DenyPost(reviewerId, postId);
            if (deniedPost != null)
            {
                return Ok(deniedPost);
            }
            return BadRequest();
        }

        /// <summary>
        /// Get list of Posts by Major or Subject or searchValue.
        /// Use this api for filter
        /// </summary>
        /// <returns></returns>
        [HttpGet("major-subject")]
        public IActionResult GetPostByMajorAndSubject([FromQuery] int[] majorID, [FromQuery] int[] subjectID, [FromQuery] string? searchValue, [FromQuery] int currentUserId)
        {
            var postList = _postHandlers.GetAllPosts(majorID, subjectID, searchValue, currentUserId);
            if (postList == null || postList.Count == 0)
            {
                postList = new List<PostDTO>();
            }
            return Ok(postList);
        }

        /// <summary>
        /// Get list of Posts by Major or Subject or searchValue.
        /// Use this api for load page
        /// </summary>
        /// <returns></returns>
        [HttpGet("on-load")]
        public IActionResult GetPostOnLoad([FromQuery] int currentUserId)
        {
            var postList = _postHandlers.GetAllPostsOnLoad(currentUserId);
            if (postList == null || postList.Count == 0)
            {
                postList = new List<PostDTO>();
            }
            return Ok(postList);
        }

        /// <summary>
        /// Get Post which is approved by post Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{postId}")]
        public IActionResult GetPostBy(int postId, int currentUserId)
        {
            var post = _postHandlers.GetPostBy(postId, currentUserId);
            if (post == null) return NotFound(); 
            return Ok(post);
        }

        /// <summary>
        /// Get Posts which have image
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-post-has-image")]
        public IActionResult GetPostsHaveImage(int currentUserId)
        {
            var posts = _postHandlers.GetPostsHaveImage(currentUserId);
            if (posts is null || posts.Count == 0)
            {
                posts = new List<PostDTO>();
            }
            return Ok(posts);
        }

        /// <summary>
        /// Get Posts which have video
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-post-has-video")]
        public IActionResult GetPostsHaveVideo(int currentUserId)
        {
            var posts = _postHandlers.GetPostsHaveVideo(currentUserId);
            if (posts is null || posts.Count == 0)
            {
                posts = new List<PostDTO>();
            }
            return Ok(posts);
        }
        /// <summary>
        /// Get top 5 voted Posts
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        [HttpGet("top-5-voted")]
        public IActionResult GetTop5VotedPost(int currentUserId)
        {
            var posts = _postHandlers.GetTop5VotedPost(currentUserId);
            if (posts is null || posts.Count == 0)
            {
                posts = new List<PostDTO>();
            }
            return Ok(posts);
        }
    }
}
