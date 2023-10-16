using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteCommentController : ControllerBase
    {
        private readonly IVoteCommentHandlers _voteCommentHandlers;

        public VoteCommentController(IVoteCommentHandlers voteCommentHandlers) 
        {
            _voteCommentHandlers = voteCommentHandlers;
        }

        [HttpPost("VoteComment")]
        public IActionResult VoteComment(int currentUserId, int commentId, bool vote)
        {
            var successVote = _voteCommentHandlers.CreateVote(currentUserId, commentId, vote);
            if (successVote == null) return BadRequest();
            return Ok(successVote);
        }

        [HttpPut("UpdateVoteComment")]
        public IActionResult DisableUpVoteComment(int currentUserId, int commentId, bool vote) 
        {
            var UpdatedVote = _voteCommentHandlers.UpdateVote(currentUserId, commentId, vote);
            if (UpdatedVote == null) return BadRequest();
            return Ok(UpdatedVote);
        }

        [HttpGet("GetAllUsersVoteComment")]
        public IActionResult GetAllUsersBy(int commentId)
        {
            var userList = _voteCommentHandlers.GetAllUsersBy(commentId);
            if (userList == null) return NotFound(); 
            return Ok(userList);
        }

        [HttpDelete("DisableVoteComment")]
        public IActionResult DisableVoteComment(int currentUserId, int commentId)
        {
            var successDisableVote = _voteCommentHandlers.DisableVote(currentUserId, commentId);
            if (successDisableVote == null) return BadRequest();
            return Ok(successDisableVote);
        }
    }
}
