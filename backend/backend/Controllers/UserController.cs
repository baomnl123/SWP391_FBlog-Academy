using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserHandlers _userHandlers;
        private readonly IFollowUserHandlers _followUserHandlers;
        public UserController(IUserHandlers userHandlers, IFollowUserHandlers followUserHandlers)
        {
            _userHandlers = userHandlers;
            _followUserHandlers = followUserHandlers;
        }

        [HttpGet("all/enable")]
        public IActionResult getAllUsers()
        {
            var list = _userHandlers.GetAllUsers();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("all/disable")]
        public IActionResult getDisableUsers()
        {
            var list = _userHandlers.GetAllDisableUsers();
            if (list == null || list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("students-and-moderators")]
        public IActionResult GetStudentsAndModerators()
        {
            var list = _userHandlers.GetStudentsAndModerator();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("all/lecturers")]
        public IActionResult GetLecturers()
        {
            var list = _userHandlers.GetLecturers();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("{userID}")]
        public IActionResult GetUser(int userID)
        {
            var user = _userHandlers.GetUser(userID);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userHandlers.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("{currentUserID}/follower")]
        public IActionResult GetAllFollower(int currentUserID)
        {
            var listFollowers = _followUserHandlers.GetAllFollowerUsers(currentUserID);

            if (listFollowers == null || listFollowers.Count == 0)
            {
                return NotFound();
            }

            return Ok(listFollowers);
        }
        [HttpGet("{currentUserID}/following")]
        public IActionResult GetAllFollowing(int currentUserID)
        {
            var listFollowings = _followUserHandlers.GetAllFollowingUsers(currentUserID);

            if (listFollowings == null || listFollowings.Count == 0)
            {
                return NotFound();
            }

            return Ok(listFollowings);
        }

        [HttpPost("student")]
        public IActionResult CreateUser([FromForm] string name, [FromForm] string avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateUser(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPost("lecturer")]
        public IActionResult CreateLecturer([FromForm] string name, [FromForm] string avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateLecturer(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
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
        [HttpPut()]
        public IActionResult UpdateUser([FromForm] int userID, [FromForm] string avatarUrl, [FromForm] string name, [FromForm] string? password)
        {
            var user = _userHandlers.UpdateUser(userID, name, avatarUrl, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPut("{userID}/promote")]
        public IActionResult PromoteStudent(int userID)
        {
            var user = _userHandlers.PromoteStudent(userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPut("{userID}/demote")]
        public IActionResult DemoteStudent(int userID)
        {
            var user = _userHandlers.DemoteStudent(userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPut("{userID}/award")]
        public IActionResult GiveAward(int userID)
        {
            var user = _userHandlers.GiveAward(userID);
            if (user == null || !user.Status)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpDelete("{userID}/award")]
        public IActionResult RemoveAward(int userID)
        {
            var user = _userHandlers.RemoveAward(userID);
            if(user == null || !user.Status)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpDelete("{userID}")]
        public IActionResult disableUser(int userID)
        {
            var user = _userHandlers.DisableUser(userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpDelete("follow")]
        public IActionResult Unfollow([FromForm] int currentUserID, [FromForm] int userID)
        {
            var user = _followUserHandlers.UnfollowUser(currentUserID, userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
