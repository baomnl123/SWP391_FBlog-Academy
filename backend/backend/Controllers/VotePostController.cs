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
        /// Get list of Users that up vote selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("all-user-up-vote/{postId}")]
        public IActionResult GetAllUsersVotedBy(int postId)
        {
            var userList = _votePostHandlers.GetAllUsersVotedBy(postId);
            if (userList == null || userList.Count == 0)
            {
                userList = new List<UserDTO>();
            }
            return Ok(userList);
        }

        /// <summary>
        /// Get list of Users that down vote selected Post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("all-user-down-vote/{postId}")]
        public IActionResult GetAllUsersDownVotedBy(int postId)
        {
            var userList = _votePostHandlers.GetAllUsersDownVotedBy(postId);
            if (userList == null || userList.Count == 0)
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
        public IActionResult CreateNewVote(int currentUserId, int postId, [FromForm] string vote) 
        {
            var boolVote = true;
            if (vote.ToLower().Trim().Contains("true"))
            {
            }
            else if (vote.ToLower().Trim().Contains("false"))
            {
                boolVote = false;
            }
            else
            {
                return BadRequest();
            }
            var createdVote = _votePostHandlers.CreateNewVotePost(currentUserId, postId, boolVote);
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
        public IActionResult UpdateVote(int currentUserId, int postId, [FromForm] string vote)
        {
            var boolVote = true;
            if (vote.ToLower().Trim().Contains("true"))
            {
            }
            else if (vote.ToLower().Trim().Contains("false"))
            {
                boolVote = false;
            }
            else
            {
                return BadRequest();
            }
            var updatedVote = _votePostHandlers.UpdateVotePost(currentUserId, postId, boolVote);
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
