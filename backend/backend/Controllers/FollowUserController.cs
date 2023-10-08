using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowUserController : ControllerBase
    {
        private readonly IFollowUserHandlers _followUserHandlers;
        public FollowUserController(IFollowUserHandlers followUserHandlers)
        {
            _followUserHandlers = followUserHandlers;
        }

        [HttpGet("/{currentUserID}/follower")]
        public IActionResult GetAllFollower(int currentUserID)
        {
            var listFollowers = (List<UserDTO>)_followUserHandlers.GetAllFollowerUsers(currentUserID);
            if (listFollowers == null || listFollowers.Count == 0)
            {
                return NotFound();
            }
            return Ok(listFollowers);
        }
        [HttpGet("{currentUserID}/following")]
        public IActionResult GetAllFollowing(int currentUserID)
        {
            var listFollowings = (List<UserDTO>)_followUserHandlers.GetAllFollowingUsers(currentUserID);
            if (listFollowings == null || listFollowings.Count == 0)
            {
                return NotFound();
            }
            return Ok(listFollowings);
        }
        [HttpPost("follow")]
        public IActionResult Follow([FromForm] int currentUserID, [FromForm] int userID)
        {
            var followRelationship = _followUserHandlers.FollowOtherUser(currentUserID, userID);
            if (followRelationship == null)
            {
                return BadRequest();
            }
            return Ok(followRelationship);
        }
        [HttpDelete("follow")]
        public IActionResult Unfollow([FromForm] int currentUserID, [FromForm] int userID)
        {
            var user = _followUserHandlers.UnfollowUser(currentUserID, userID);
            if(user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
