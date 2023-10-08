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
        [HttpGet("AllPosts")]
        public IActionResult GetAllPosts()
        {
            var existed = _postHandlers.GetAllPosts();
            if (existed != null) { return Ok(existed); }
            return NotFound();
        }

        [HttpGet("Posts")]
        public IActionResult SearchPostsByContent(string content)
        {
            //Search Post's list
            var existed = _postHandlers.SearchPostsByContent(content);

            //check not null to return
            if(existed != null)
            {
                return Ok(existed.ToList());
            }
            return NotFound();
        }
    }
}
