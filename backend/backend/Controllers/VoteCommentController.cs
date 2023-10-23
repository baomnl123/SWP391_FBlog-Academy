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

        [HttpPost]
        public IActionResult VoteComment([FromForm] int currentUserId, [FromForm] int commentId, [FromForm] bool vote)
        {
            var successVote = _voteCommentHandlers.CreateVote(currentUserId, commentId, vote);
            if (successVote == null) return BadRequest();
            return Ok(successVote);
        }

        [HttpPut]
        public IActionResult DisableUpVoteComment([FromForm] int currentUserId, [FromForm] int commentId, [FromForm] bool vote) 
        {
            var UpdatedVote = _voteCommentHandlers.UpdateVote(currentUserId, commentId, vote);
            if (UpdatedVote == null) return BadRequest();
            return Ok(UpdatedVote);
        }

        [HttpGet("{commentId}")]
        public IActionResult GetAllUsersVotedBy(int commentId)
        {
            var userList = _voteCommentHandlers.GetAllUsersVotedBy(commentId);
            if (userList == null) return NotFound(); 
            return Ok(userList);
        }

        [HttpDelete]
        public IActionResult DisableVoteComment([FromForm] int currentUserId, [FromForm] int commentId)
        {
            var successDisableVote = _voteCommentHandlers.DisableVote(currentUserId, commentId);
            if (successDisableVote == null) return BadRequest();
            return Ok(successDisableVote);
        }
    }
}
