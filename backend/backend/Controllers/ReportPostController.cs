using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportPostController : ControllerBase
    {
        private IReportPostHandlers _reportPostHandlers;
        public ReportPostController(IReportPostHandlers reportPostHandlers) { 
            _reportPostHandlers = reportPostHandlers;
        }
        [HttpGet]
        public IActionResult GetAllReportPost()
        {
            List<ReportPostDTO> reportPostList = (List<ReportPostDTO>)_reportPostHandlers.GetAllPendingReportPost();

            if(reportPostList == null || reportPostList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(reportPostList);
            }
        }
        [HttpPost]
        public IActionResult AddReportPost([FromBody]int reporterID, int postID, string content)
        {
            var reportPost = AddReportPost(reporterID,postID,content);

            if(reportPost == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(reportPost);
            }
        }
        [HttpPut("content")]
        public IActionResult UpdateReportContent([FromBody]int reportPostID,string content)
        {
            ReportPostDTO reportPostDTO = _reportPostHandlers.UpdateReportPost(reportPostID, content);
            if(reportPostDTO == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(reportPostDTO);
            }
        }
        [HttpPut("status")]
        public IActionResult UpdateReportStatus([FromBody] int reportPostID, string status)
        {
            ReportPostDTO reportPostDTO = _reportPostHandlers.UpdateReportPostStatus(reportPostID, status);
            if (reportPostDTO == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(reportPostDTO);
            }
        }
        [HttpDelete]
        public IActionResult DisableReportPost(int reportPostID)
        {
            if (!_reportPostHandlers.DenyReportPost(reportPostID))
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
