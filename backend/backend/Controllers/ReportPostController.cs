using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportPostController : ControllerBase
    {
        private IReportPostHandlers _reportPostHandlers;
        private EmailSender _emailSender;
        public ReportPostController(IReportPostHandlers reportPostHandlers)
        {
            _reportPostHandlers = reportPostHandlers;
            _emailSender = new EmailSender();
        }
        /// <summary>
        /// Get list of Report Posts. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllReportPost()
        {
            var reportPostList = _reportPostHandlers.GetAllReportPost();
            if (reportPostList == null || reportPostList.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(reportPostList);
        }

        /// <summary>
        /// Get list of pending Report Post. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("pending")]
        public IActionResult GetAllPendingReportPost()
        {
            var reportPostList = _reportPostHandlers.GetAllPendingReportPost();

            if (reportPostList == null || reportPostList.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(reportPostList);
        }

        /// <summary>
        /// Get list of Report Post of selected User. (Admin)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("{userID}")]
        public IActionResult GetAllPendingReportPost(int userID)
        {
            var reportPostList = _reportPostHandlers.GetAllPendingReportPost(userID);

            if (reportPostList == null || reportPostList.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(reportPostList);
        }

        /// <summary>
        /// Create New Report Post. (Student | Moderator)
        /// </summary>
        /// <param name="reporterID"></param>
        /// <param name="postID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddReportPost(int reporterID, int postID, [FromForm] string? content)
        {
            var reportPost = _reportPostHandlers.AddReportPost(reporterID, postID, content);

            if (reportPost == null)
            {
                return BadRequest();
            }
            return Ok(reportPost);
        }

        /// <summary>
        /// Update content of selected Report Post. (Student | Moderator)
        /// </summary>
        /// <param name="reportPostID"></param>
        /// <param name="postID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPut("content")]
        public IActionResult UpdateReportContent(int reporterID, int postID, [FromForm] string? content)
        {
            var reportPostDTO = _reportPostHandlers.UpdateReportPost(reporterID, postID, content);
            if (reportPostDTO == null)
            {
                return BadRequest();
            }
            return Ok(reportPostDTO);
        }

        /// <summary>
        /// Update status of the Report Post. (Admin)
        /// </summary>
        /// <param name="adminID"></param>
        /// <param name="reporterID"></param>
        /// <param name="postID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public async Task<IActionResult> ApproveReportPost(int adminID, int reporterID, int postID)
        {
            var reportPostDTO = _reportPostHandlers.ApproveReportPost(adminID, reporterID, postID);
            if (reportPostDTO == null)
            {
                return BadRequest();
            }
            return Ok(reportPostDTO);
        }

        /// <summary>
        /// Disable Report Post. (Admin)
        /// </summary>
        /// <param name="adminID"></param>
        /// <param name="reporterID"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DisableReportPost(int adminID, int reporterID, int postID)
        {
            var reportPost = _reportPostHandlers.DenyReportPost(adminID, reporterID, postID);
            if(reportPost == null)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = reportPost.Reporter.Email;
            var existedSubject = $"Your report has been denied !";
            var existedMessage = $"The admin has reviewed your Report and eventually deny your Report !\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(reportPost);
        }
    }
}
