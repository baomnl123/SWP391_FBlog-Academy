using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentHandlers _commentHandlers;

        public CommentController(ICommentHandlers commentHandlers)
        {
            _commentHandlers = commentHandlers;
        }

        [HttpGet("ViewAllComments")]
        public IActionResult GetAllComments(int postId)
        {
            var commentList = _commentHandlers.ViewAllComments(postId);
            if (commentList != null) return Ok(commentList);
            return NotFound();
        }

        [HttpPost("CreatNewComment")]
        public IActionResult CreateNewComment(int userId, int postId, string content) 
        {
            var newComment = _commentHandlers.CreateComment(userId, postId, content);
            if (newComment != null) return Ok(newComment);
            return BadRequest();
        }

        [HttpPut("UpdateComment")]
        public IActionResult UpdateComment(int commentId, string content)
        {
            var updatedComment = _commentHandlers.UpdateComment(commentId , content);
            if (updatedComment != null) return Ok(updatedComment);
            return BadRequest();
        }

        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int commentId)
        {
            var deletedComment = _commentHandlers.DeleteComment(commentId);
            if (deletedComment != null) return Ok(deletedComment);
            return BadRequest();
        }
    }
}
