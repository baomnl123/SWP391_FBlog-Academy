using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotePostController : ControllerBase
    {
        private readonly IVotePostHandlers _votePostHandlers;
        public VotePostController(IVotePostHandlers votePostHandlers) 
        {
            _votePostHandlers = votePostHandlers;
        }

        /// <summary>
        /// Get list of Users that vote selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        public IActionResult GetAllUsersVotedBy(int postId)
        {
            var userList = _votePostHandlers.GetAllUsersVotedBy(postId);
            if (userList == null)
            {
                userList = new List<UserDTO>();
            }
            return Ok(userList);
        }

        /// <summary>
        /// Vote selected Post. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="postId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewVote(int currentUserId, int postId, [FromForm] bool vote) 
        {
            var createdVote = _votePostHandlers.CreateNewVotePost(currentUserId, postId, vote);
            if (createdVote == null) return BadRequest();
            return Ok(createdVote);
        }

        /// <summary>
        /// Change to from upvote to downvote or from downvote to upvote (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="postId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateVote(int currentUserId, int postId, [FromForm] bool vote)
        {
            var updatedVote = _votePostHandlers.UpdateVotePost(currentUserId, postId, vote);
            if (updatedVote == null) return BadRequest();
            return Ok(updatedVote);
        }

        /// <summary>
        /// Change to not Vote (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteVote(int currentUserId, int postId) 
        {
            var deletedVote = _votePostHandlers.DisableVotePost(currentUserId, postId);
            if (deletedVote == null) return BadRequest();
            return Ok(deletedVote);
        }
    }
}
