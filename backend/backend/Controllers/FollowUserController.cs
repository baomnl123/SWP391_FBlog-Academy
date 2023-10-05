using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowUserController : ControllerBase
    {
        private IFollowUserHandlers _followUserHandlers;
        public FollowUserController(IFollowUserHandlers followUserHandlers)
        {
            _followUserHandlers = followUserHandlers;
        }

        [HttpGet("follower")]
        public IActionResult GetAllFollower(int currentUserID)
        {
            List<UserDTO> listFollowers = (List<UserDTO>)_followUserHandlers.GetAllFollowerUsers(currentUserID);
            if (listFollowers == null || listFollowers.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(listFollowers);
            }
        }
        [HttpGet("followed")]
        public IActionResult GetAllFollowing(int currentUserID)
        {
            List<UserDTO> listFollowings = (List<UserDTO>)_followUserHandlers.GetAllFollowingUsers(currentUserID);
            if (listFollowings == null || listFollowings.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(listFollowings);
            }
        }
        [HttpPost("follow")]
        public IActionResult Follow([FromQuery] int currentUserID, int userID)
        {
            if (!_followUserHandlers.FollowOtherUser(currentUserID, userID))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
        [HttpPost("unfollow")]
        public IActionResult Unfollow([FromQuery] int currentUserID, int userID)
        {
            if (!_followUserHandlers.UnfollowUser(currentUserID, userID))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
    }
}
