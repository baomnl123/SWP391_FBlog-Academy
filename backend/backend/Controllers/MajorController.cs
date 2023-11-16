using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : Controller
    {
        private readonly IMajorHandlers _majorHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly UserRoleConstrant _userRoleConstrant;

        public MajorController(IMajorHandlers majorHandlers, IUserHandlers userHandlers)
        {
            _majorHandlers = majorHandlers;
            _userHandlers = userHandlers;
            _userRoleConstrant = new();
        }

        /// <summary>
        /// Get all the Majors that are available. (Admin) 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Major>))]
        public IActionResult GetMajors()
        {
            var categories = _majorHandlers.GetMajors();

            if (categories == null)
            {
                var emptyList = new List<MajorDTO>();
                return Ok(emptyList);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Get all the Majors that are disable. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("disable")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Major>))]
        public IActionResult GetDisableMajors()
        {
            var categories = _majorHandlers.GetDisableMajors();

            if (categories == null)
            {
                var emptyList = new List<MajorDTO>();
                return Ok(emptyList);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Get Major with selected MajorID. 
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        [HttpGet("{majorId}")]
        [ProducesResponseType(200, Type = typeof(Major))]
        [ProducesResponseType(400)]
        public IActionResult GetMajor(int majorId)
        {
            var major = _majorHandlers.GetMajorById(majorId);
            if (major == null || major.Status == false) return NotFound();

            return Ok(major);
        }

        /// <summary>
        /// Get all Posts that have the selected Major. (Admin)
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        [HttpGet("{majorId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Major>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsByMajor(int majorId)
        {
            var posts = _majorHandlers.GetPostsByMajor(majorId);
            if (posts == null)
            {
                var emptyList = new List<PostDTO>();
                return Ok(emptyList);
            }

            return Ok(posts);
        }

        /// <summary>
        /// Get all Subjects that have the selected Major. 
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        [HttpGet("{majorId}/subjects")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Major>))]
        [ProducesResponseType(400)]
        public IActionResult GetSubjectsByMajor(int majorId)
        {
            var subjects = _majorHandlers.GetSubjectsByMajor(majorId);
            if (subjects == null)
            {
                var emptyList = new List<SubjectDTO>();
                return Ok(emptyList);
            }

            return Ok(subjects);
        }

        /// <summary>
        /// Create a new Major. (Admin)
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="majorName"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateMajor(int adminId, [FromForm] string majorName)
        {
            var admin = _userHandlers.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (admin == null || admin.Role != adminRole)
                return NotFound("Admin does not exists!");

            var major = _majorHandlers.GetMajorByName(majorName);
            if (major != null)
            {
                // If major already exists, and status is true, then return 
                if (major.Status) return StatusCode(422, $"\"{major.MajorName}\" aldready exists!");

                // If major already exists, but was disabled, then enable it
                return Ok(_majorHandlers.EnableMajor(major.Id));
            }

            var createMajor = _majorHandlers.CreateMajor(adminId, majorName);
            if (createMajor == null) return BadRequest(ModelState);

            return Ok(createMajor);
        }

        /// <summary>
        /// Update the selected Major. (Admin)
        /// </summary>
        /// <param name="newMajorName"></param>
        /// <param name="currentMajorId"></param>
        /// <returns></returns>
        [HttpPut("{currentMajorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMajor([FromForm] string newMajorName, int currentMajorId)
        {
            // If major does not exists for updating, return Not found
            var currentMajor = _majorHandlers.GetMajorById(currentMajorId);
            if (currentMajor == null) return NotFound("Major does not exists!");

            // Check the new major name already exists in DB
            var isMajorExists = _majorHandlers.GetMajorByName(newMajorName);
            if (isMajorExists != null && isMajorExists.Status)
                return StatusCode(422, $"\"{isMajorExists.MajorName}\" aldready exists!");

            // If major already exists, but was disabled, then enable it
            if (isMajorExists != null && !isMajorExists.Status)
            {
                _majorHandlers.EnableMajor(isMajorExists.Id);
                return StatusCode(422, $"\"{isMajorExists.MajorName}\" aldready exists!");
            }

            // If the new name does not exists, then update to the current major    
            var updateMajor = _majorHandlers.UpdateMajor(currentMajorId, newMajorName);
            if (updateMajor == null) return BadRequest();

            return Ok(updateMajor);
        }

        /// <summary>
        /// Enable the selected "disabled" Major. (Admin)
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        [HttpPut("enable/{majorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableMajor(int majorId)
        {
            var major = _majorHandlers.GetMajorById(majorId);
            if (major == null) return NotFound("Major does not exists!");

            var enableMajor = _majorHandlers.EnableMajor(majorId);
            if (enableMajor == null)
                ModelState.AddModelError("", "Something went wrong enable major");

            return Ok(enableMajor);
        }

        /// <summary>
        /// Delete the selected Major. (Admin)
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        [HttpDelete("{majorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMajor(int majorId)
        {
            var major = _majorHandlers.GetMajorById(majorId);
            if (major == null) return NotFound("Major does not exists!");

            var deleteMajor = _majorHandlers.DisableMajor(majorId);
            if (deleteMajor == null)
                ModelState.AddModelError("", "Something went wrong deleting major");

            return Ok(deleteMajor);
        }

        /// <summary>
        /// Get top 5 voted Majors
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-5-voted")]
        public IActionResult GetTop5VotedPost()
        {
            var posts = _majorHandlers.GetTop5Majors();
            if (posts is null || posts.Count == 0)
            {
                posts = new List<MajorDTO>();
            }
            return Ok(posts);
        }

    }
}
