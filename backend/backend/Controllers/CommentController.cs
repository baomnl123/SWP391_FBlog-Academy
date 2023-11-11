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
    public class CommentController : ControllerBase
    {
        private readonly ICommentHandlers _commentHandlers;
        private readonly EmailSender _emailSender;

        public CommentController(ICommentHandlers commentHandlers)
        {
            _commentHandlers = commentHandlers;
            _emailSender = new EmailSender();
        }

        /// <summary>
        /// Get all Comments by selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}/comments")]
        public IActionResult GetAllComments(int postId, int currentUserId)
        {
            var commentList = _commentHandlers.ViewAllComments(postId, currentUserId);
            if (commentList == null || commentList.Count == 0)
            {
                commentList = new List<CommentDTO>();
            }
            return Ok(commentList);
        }
        /// <summary>
        /// Create a new Comment for selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewComment(int userId, int postId, [FromForm] string content)
        {
            var newComment = _commentHandlers.CreateComment(userId, postId, content);
            if (newComment != null)
            {
                //send email
                var existedEmail = newComment.Post.User.Email;
                var existedSubject = $"Someone has commented to your post.";
                var existedMessage = $"{newComment.User.Name} has commented to {newComment.Post.Title}.\n\nFaithfully,FBlog Academy";

                await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
                return Ok(newComment);
            }
            return BadRequest();
        }

        /// <summary>
        /// Update the selected Comment. (Student | Moderator)
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        [HttpPut("{commentId}")]
        public IActionResult UpdateComment(int commentId, [FromForm] string content)
        {
            var updatedComment = _commentHandlers.UpdateComment(commentId, content);
            if (updatedComment != null) return Ok(updatedComment);
            return BadRequest();
        }

        /// <summary>
        /// Delete the selected Comment (Student | Moderator)
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            var deletedComment = _commentHandlers.DeleteComment(commentId);
            if (deletedComment != null) return Ok(deletedComment);
            return BadRequest();
        }

        /// <summary>
        /// Get Comment which is not disabled by comment Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{postId}")]
        public IActionResult GetCommentBy(int commentId)
        {
            var comment = _commentHandlers.GetCommentBy(commentId);
            if (comment == null) return NotFound();
            return Ok(comment);
        }
    }
}
