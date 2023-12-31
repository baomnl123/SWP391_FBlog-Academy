﻿using backend.DTO;
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

        /// <summary>
        /// Vote a Comment (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="commentId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult VoteComment(int currentUserId, int commentId, int vote)
        {
            if (vote < 1 || vote > 2) return BadRequest("Invalid input!");
            var successVote = _voteCommentHandlers.CreateVote(currentUserId, commentId, vote);
            if (successVote == null) return BadRequest();
            return Ok(successVote);
        }

        /// <summary>
        /// Change to from upvote to downvote or from downvote to upvote (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="commentId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult DisableUpVoteComment(int currentUserId, int commentId, int vote) 
        {
            if (vote < 1 || vote > 2) return BadRequest("Invalid input!");
            var UpdatedVote = _voteCommentHandlers.UpdateVote(currentUserId, commentId, vote);
            if (UpdatedVote == null) return BadRequest();
            return Ok(UpdatedVote);
        }

        /// <summary>
        /// Get list of Users that vote that Comments. 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpGet("{commentId}")]
        public IActionResult GetAllUsersVotedBy(int commentId)
        {
            var userList = _voteCommentHandlers.GetAllUsersVotedBy(commentId);
            if (userList == null || userList.Count == 0)
            {
                userList = new List<UserDTO>();
            }
            return Ok(userList);
        }

        /// <summary>
        /// Change to not Vote at all. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DisableVoteComment(int currentUserId, int commentId)
        {
            var successDisableVote = _voteCommentHandlers.DisableVote(currentUserId, commentId);
            if (successDisableVote == null) return BadRequest();
            return Ok(successDisableVote);
        }
    }
}
