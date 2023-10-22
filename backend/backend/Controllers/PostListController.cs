using backend.Handlers.IHandlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostListController : ControllerBase
    {
        private readonly IPostListHandlers _postListHandlers;
        public PostListController(IPostListHandlers postListHandlers)
        {
            _postListHandlers = postListHandlers;
        }
        [HttpGet("savelists")]
        public IActionResult GetSaveListByPostID(int postID, int userID)
        {
            var savelists = _postListHandlers.GetAllSaveListByPostID(postID, userID);
            if (savelists == null || savelists.Count == 0)
            {
                return NotFound();
            }
            return Ok(savelists);
        }
        [HttpGet("{saveListID}")]
        public IActionResult GetAllPostBySaveList(int saveListID)
        {
            var postList = _postListHandlers.GetAllPostBySaveListID(saveListID);
            if (postList == null)
            {
                return NotFound();
            }
            return Ok(postList);
        }
        [HttpPost]
        public IActionResult AddPostList(int saveListID, int postID)
        {
            var postList = _postListHandlers.AddPostList(saveListID, postID);
            if (postList == null)
            {
                return BadRequest();
            }
            return Ok(postList);
        }
        [HttpDelete]
        public IActionResult DeletePostList(int saveListID, int postID)
        {
            var postList = _postListHandlers.DisablePostList(saveListID, postID);
            if (postList == null)
            {
                return BadRequest();
            }
            return Ok(postList);
        }
    }
}
