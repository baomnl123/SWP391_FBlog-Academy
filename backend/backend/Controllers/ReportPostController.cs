using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportPostController : ControllerBase
    {
        private IReportPostHandlers _reportPostHandlers;
        public ReportPostController(IReportPostHandlers reportPostHandlers)
        {
            _reportPostHandlers = reportPostHandlers;
        }
        [HttpGet]
        public IActionResult GetAllPendingReportPost()
        {
            List<ReportPostDTO> reportPostList = (List<ReportPostDTO>)_reportPostHandlers.GetAllPendingReportPost();

            if (reportPostList == null || reportPostList.Count == 0)
            {
                return NotFound();
            }
            return Ok(reportPostList);
        }
        [HttpPost]
        public IActionResult AddReportPost([FromForm] int reporterID, [FromForm] int postID, [FromForm] string? content)
        {
            var reportPost = _reportPostHandlers.AddReportPost(reporterID, postID, content);

            if (reportPost == null)
            {
                return BadRequest();
            }
            return Ok(reportPost);
        }
        [HttpPut("content")]
        public IActionResult UpdateReportContent([FromForm] int reportPostID, [FromForm] int postID, [FromForm] string? content)
        {
            var reportPostDTO = _reportPostHandlers.UpdateReportPost(reportPostID, postID, content);
            if (reportPostDTO == null)
            {
                return BadRequest();
            }
            return Ok(reportPostDTO);
        }
        [HttpPut("status")]
        public IActionResult UpdateReportStatus([FromForm] int reportPostID, [FromForm] int postID, [FromForm] string status)
        {
            var reportPostDTO = _reportPostHandlers.UpdateReportStatus(reportPostID, postID, status);
            if (reportPostDTO == null)
            {
                return BadRequest();
            }
            return Ok(reportPostDTO);
        }
        [HttpDelete]
        public IActionResult DisableReportPost([FromForm] int reportPostID, [FromForm] int postID)
        {
            if (!_reportPostHandlers.DenyReportPost(reportPostID, postID))
            {
                return BadRequest();
            }
                return Ok();
        }
    }
}
