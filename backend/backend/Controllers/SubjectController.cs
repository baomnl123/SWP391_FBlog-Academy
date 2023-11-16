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
    public class SubjectController : Controller
    {
        private readonly ISubjectHandlers _subjectHandlers;
        private readonly IMajorHandlers _majorHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly UserRoleConstrant _userRoleConstrant;

        public SubjectController(ISubjectHandlers subjectHandlers, IMajorHandlers majorHandlers, IUserHandlers userHandlers)
        {
            _subjectHandlers = subjectHandlers;
            _majorHandlers = majorHandlers;
            _userHandlers = userHandlers;
            _userRoleConstrant = new();
        }

        /// <summary>
        /// Get list of enable Subjects. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subject>))]
        public IActionResult GetSubjects()
        {
            var subjects = _subjectHandlers.GetSubjects();

            if (subjects == null)
            {
                var emptyList = new List<SubjectDTO>();
                return Ok(emptyList);
            }

            return Ok(subjects);
        }

        /// <summary>
        /// Get list of enable Subjects. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("disable")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subject>))]
        public IActionResult GetDisableSubjects()
        {
            var subjects = _subjectHandlers.GetDisableSubjects();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(subjects);
        }

        /// <summary>
        /// Get selected Subject by ID.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("{subjectId}")]
        [ProducesResponseType(200, Type = typeof(Subject))]
        [ProducesResponseType(400)]
        public IActionResult GetSubject(int subjectId)
        {
            var subject = _subjectHandlers.GetSubjectById(subjectId);
            if (subject == null || subject.Status == false) return NotFound();

            return Ok(subject);
        }

        /// <summary>
        /// Get list of Posts that contains selected Subject. (Admin)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("{subjectId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subject>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsBySubject(int subjectId)
        {
            var posts = _subjectHandlers.GetPostsBySubject(subjectId);
            if (posts == null)
            {
                var emptyList = new List<PostDTO>();
                return Ok(emptyList);
            }

            return Ok(posts);
        }

        /// <summary>
        /// Get list of majors that contains selected Subject.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("{subjectId}/majors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Subject>))]
        [ProducesResponseType(400)]
        public IActionResult GetMajorsBySubject(int subjectId)
        {
            var majors = _subjectHandlers.GetMajorsBySubject(subjectId);
            if (majors == null)
            {
                var emptyList = new List<MajorDTO>();
                return Ok(emptyList);
            }

            return Ok(majors);
        }

        /// <summary>
        /// Create a new Subject. (Admin)
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="majorId"></param>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateSubject(int adminId, int majorId, [FromForm] string subjectName)
        {
            // Check if admin exists
            var admin = _userHandlers.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (admin == null || admin.Role != adminRole)
                return NotFound("Admin does not exists!");

            // Check if major does not exists
            var major = _majorHandlers.GetMajorById(majorId);
            if (major == null || !major.Status) return NotFound("Major does not exists!");

            var isSubjectExists = _subjectHandlers.GetSubjectByName(subjectName);
            if (isSubjectExists != null && isSubjectExists.Status)
            {
                if (_subjectHandlers.CreateMajorSubject(isSubjectExists, major) != null)
                    return Ok(isSubjectExists);

                return StatusCode(422, $"\"{isSubjectExists.SubjectName}\" already exists!");
            }

            if (isSubjectExists != null && !isSubjectExists.Status)
            {
                // If the subject was disabled, enable it then create relationship with Major
                _subjectHandlers.EnableSubject(isSubjectExists.Id);
                if (_subjectHandlers.CreateMajorSubject(isSubjectExists, major) != null)
                    return Ok(isSubjectExists);

                return StatusCode(422, $"\"{isSubjectExists.SubjectName}\" already exists!");
            }

            // If subject is null, then create new subject
            var createSubject = _subjectHandlers.CreateSubject(adminId, majorId, subjectName);
            if (createSubject == null) return BadRequest(ModelState);

            // If create succeed, then create relationship
            _subjectHandlers.CreateMajorSubject(createSubject, major);

            return Ok(createSubject);
        }

        /// <summary>
        /// Update selected Subject. (Admin)
        /// </summary>
        /// <param name="newSubjectName"></param>
        /// <param name="currentSubjectId"></param>
        /// <returns></returns>
        [HttpPut("{currentSubjectId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSubject([FromForm] string newSubjectName, int currentSubjectId)
        {
            // If subject does not exists for updating, return Not found
            var currentSubject = _subjectHandlers.GetSubjectById(currentSubjectId);
            if (currentSubject == null || !currentSubject.Status) return NotFound("Subject does not exists!");

            // Check the new subject name already exists in DB
            var isSubjectExists = _subjectHandlers.GetSubjectByName(newSubjectName);
            if (isSubjectExists != null && isSubjectExists.Status)
                return StatusCode(422, $"\"{isSubjectExists.SubjectName}\" aldready exists!");

            // If subject already exists, but was disabled, then enable it
            if (isSubjectExists != null && !isSubjectExists.Status)
            {
                _subjectHandlers.EnableSubject(isSubjectExists.Id);
                return StatusCode(422, $"\"{isSubjectExists.SubjectName}\" aldready exists!");
            }

            // If the new name does not exists, then update to the current subject
            var updateSubjecte = _subjectHandlers.UpdateSubject(currentSubjectId, newSubjectName);
            if (updateSubjecte == null) return BadRequest();

            return Ok(updateSubjecte);
        }

        /// <summary>
        /// Enable selected Subject. (Admin)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPut("enable/{subjectId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableSubject(int subjectId)
        {
            var enableSubject = _subjectHandlers.EnableSubject(subjectId);
            if (enableSubject == null) ModelState.AddModelError("", "Something went wrong enable subject");

            return Ok(enableSubject);
        }

        /// <summary>
        /// Delete selected Subject. (Admin)
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpDelete("{subjectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSubject(int subjectId)
        {
            var deleteSubject = _subjectHandlers.DisableSubject(subjectId);
            if (deleteSubject == null) ModelState.AddModelError("", "Something went wrong delete subject");

            return Ok(deleteSubject);
        }

        /// <summary>
        /// Remove selected Subject from selected Major. (Admin)
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpDelete("{majorId}/{subjectId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRelationship(int majorId, int subjectId)
        {
            var deleteRelationship = _subjectHandlers.DisableMajorSubject(subjectId, majorId);
            if (deleteRelationship == null) ModelState.AddModelError("", "Something went wrong delete relationship");

            return Ok(deleteRelationship);
        }

        /// <summary>
        /// Get top 5 voted Subjects
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-5-voted")]
        public IActionResult GetTop5VotedSubject()
        {
            var posts = _subjectHandlers.GetTop5Subjects();
            if (posts is null || posts.Count == 0)
            {
                posts = new List<SubjectDTO>();
            }
            return Ok(posts);
        }
    }
}
