using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostHandlers _postHandlers;

        public PostController(IPostHandlers postHandlers)
        {
            _postHandlers = postHandlers;
        }
        [HttpGet("Posts")]
        public IActionResult SearchPostsByContent(string content)
        {
            //Search Post's list
            var existed = _postHandlers.SearchPostsByContent(content);

            //check not null to return
            if(existed != null)
            {
                List<PostDTO> listResult = existed.ToList();
                return Ok(listResult);
            }
            return NotFound();
        }
    }
}
