using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotePostController : ControllerBase
    {
        private readonly IVotePostHandlers _votePostHandlers;
        private readonly EmailSender _emailSender;
        public VotePostController(IVotePostHandlers votePostHandlers)
        {
            _votePostHandlers = votePostHandlers;
            _emailSender = new EmailSender();
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
        public async Task<IActionResult> CreateNewVote(int currentUserId, int postId, [FromForm] int vote)
        {
            if (vote < 1 && vote > 2) return BadRequest();

            var createdVote = _votePostHandlers.CreateNewVotePost(currentUserId, postId, vote);
            if (createdVote == null) return BadRequest();
            if (createdVote.Vote != 0)
            {
                //send email
                 var existedEmail = createdVote.Post.User.Email;
                var existedSubject = $"{createdVote.User.Name} has voted your post.";
                var existedMessage = $"{createdVote.User.Name} has voted {createdVote.Post.Title}.\n\nFaithfully,FBlog Academy";

                await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            }

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
        public async Task<IActionResult> UpdateVote(int currentUserId, int postId, [FromForm] int vote)
        {
            if (vote < 1 && vote > 2) return BadRequest();

            var updatedVote = _votePostHandlers.UpdateVotePost(currentUserId, postId, vote);
            if (updatedVote == null) return BadRequest();

            if (updatedVote.Vote == 1)
            {
                //send email
                var existedEmail = updatedVote.Post.User.Email;
                var existedSubject = $"{updatedVote.User.Name} has voted your post.";
                var existedMessage = $"{updatedVote.User.Name} has voted {updatedVote.Post.Title}.\n\nFaithfully,FBlog Academy";

                await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            }
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
