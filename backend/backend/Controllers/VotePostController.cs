﻿using backend.Handlers.IHandlers;
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

        [HttpGet("SearchUsersVotedBy/{postId}")]
        public IActionResult GetAllUsersVotedBy(int postId)
        {
            var userList = _votePostHandlers.GetAllUsersVotedBy(postId);
            if (userList == null) return NotFound();
            return Ok(userList);
        }

        [HttpPost("AddNewVotePost")]
        public IActionResult CreateNewVote([FromForm] int currentUserId, [FromForm] int postId, [FromForm] bool vote) 
        {
            var createdVote = _votePostHandlers.CreateNewVotePost(currentUserId, postId, vote);
            if (createdVote == null) return BadRequest();
            return Ok(createdVote);
        }

        [HttpPut("UpdateVotePost")]
        public IActionResult UpdateVote([FromForm] int currentUserId, [FromForm] int postId, [FromForm] bool vote)
        {
            var updatedVote = _votePostHandlers.UpdateVotePost(currentUserId, postId, vote);
            if (updatedVote == null) return BadRequest();
            return Ok(updatedVote);
        }

        [HttpDelete("DeleteVotePost")]
        public IActionResult DeleteVote(int currentUserId, int postId) 
        {
            var deletedVote = _votePostHandlers.DisableVotePost(currentUserId, postId);
            if (deletedVote == null) return BadRequest();
            return Ok(deletedVote);
        }
    }
}