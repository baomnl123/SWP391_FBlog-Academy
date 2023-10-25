using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserHandlers _userHandlers;
        public UserController(IUserHandlers userHandlers)
        {
            _userHandlers = userHandlers;
        }
        
        [HttpGet("all/enable")]
        public IActionResult getAllUsers()
        {
            var list = (List<UserDTO>)_userHandlers.GetAllUsers();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("all/disable")]
        public IActionResult getDisableUsers() {
            var list = _userHandlers.GetAllDisableUsers();
            if(list == null || list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }
        [HttpGet("students-and-moderator")]
        public IActionResult GetStudentsAndModerators()
        {
            var list = (List<UserDTO>)_userHandlers.GetStudentsAndModerator();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("all/lecturer")]
        public IActionResult GetLecturers()
        {
            var list = (List<UserDTO>)_userHandlers.GetLecturers();
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
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("student")]
        public IActionResult CreateUser([FromForm] string name,[FromForm] string avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateUser(name,avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPost("lecturer")]
        public IActionResult CreateLecturer([FromForm] string name,[FromForm] string avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateLecturer(name,avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
        [HttpPut()]
        public IActionResult UpdateUser([FromForm] int userID,[FromForm] string avatarUrl, [FromForm] string name, [FromForm] string? password)
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
        [HttpDelete("{userID}")]
        public IActionResult disableUser(int userID)
        {
            var user = _userHandlers.DisableUser(userID);
            if(user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
