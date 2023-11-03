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

        /// <summary>
        /// Get list of enable Users. (Moderator | Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/enable")]
        public IActionResult getAllUsers()
        {
            var list = _userHandlers.GetAllUsers();
            if (list == null)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get list of disable Users. (Moderator | Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/disable")]
        public IActionResult getDisableUsers()
        {
            var list = _userHandlers.GetAllDisableUsers();
            if (list == null || list.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get list of Students and Moderators. (Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("students-and-moderators")]
        public IActionResult GetStudentsAndModerators()
        {
            var list = _userHandlers.GetStudentsAndModerator();
            if (list == null)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get list of Lecturers. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/lecturers")]
        public IActionResult GetLecturers()
        {
            var list = _userHandlers.GetLecturers();
            if (list == null)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get selected User by his/her ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get selected User by his/her email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of people that follow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        [HttpGet("{currentUserID}/follower")]
        public IActionResult GetAllFollower(int currentUserID)
        {
            var listFollowers = _followUserHandlers.GetAllFollowerUsers(currentUserID);

            if (listFollowers == null || listFollowers.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }

            return Ok(listFollowers);
        }

        /// <summary>
        /// Get list of people that selected User follows. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        [HttpGet("{currentUserID}/following")]
        public IActionResult GetAllFollowing(int currentUserID)
        {
            var listFollowings = _followUserHandlers.GetAllFollowingUsers(currentUserID);

            if (listFollowings == null || listFollowings.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }

            return Ok(listFollowings);
        }

        /// <summary>
        /// Create a new User account. (Guest)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("student")]
        public IActionResult CreateUser([FromForm] string name, [FromForm] string? avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateUser(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        /// <summary>
        /// Create a new Lecturer account. (Admin)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("lecturer")]
        public IActionResult CreateLecturer([FromForm] string name, [FromForm] string? avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateLecturer(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        /// <summary>
        /// Follow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost("follow")]
        public IActionResult Follow(int currentUserID, int userID)
        {
            var followRelationship = _followUserHandlers.FollowOtherUser(currentUserID, userID);
            if (followRelationship == null)
            {
                return BadRequest();
            }
            return Ok(followRelationship);
        }

        /// <summary>
        /// Update selected User.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPut("{userID}")]
        public IActionResult UpdateUser(int userID, [FromForm] string? avatarUrl, [FromForm] string name, [FromForm] string? password)
        {
            var user = _userHandlers.UpdateUser(userID, name, avatarUrl, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        /// <summary>
        /// Promote Student to Moderator (Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Demote Moderator to Student (Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Give award to selected Student. (Moderator | Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove Award from selected Student (Moderator | Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Ban selected User. (Admin)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Unfollow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpDelete("follow")]
        public IActionResult Unfollow(int currentUserID, int userID)
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
