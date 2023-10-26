using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveListController : ControllerBase
    {
        private readonly ISaveListHandlers _saveListHandlers;
        private readonly IPostListHandlers _postListHandlers;
        public SaveListController(ISaveListHandlers saveListHandlers, IPostListHandlers postListHandlers)
        {
            _saveListHandlers = saveListHandlers;
            _postListHandlers = postListHandlers;
        }
        [HttpGet("{userID}")]
        public IActionResult GetAllSaveList(int userID)
        {
            var saveList = (List<SaveListDTO>)_saveListHandlers.GetAllActiveSaveList(userID);
            if (saveList == null || saveList.Count == 0)
            {
                return NotFound();
            }
            return Ok(saveList);
        }
        [HttpGet("{userID}/disable")]
        public IActionResult getDisableUsers(int userID)
        {
            var list = _saveListHandlers.GetAllDisableSaveList(userID);
            if (list == null || list.Count == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("posts/{saveListID}")]
        public IActionResult GetAllPostBySaveList(int saveListID)
        {
            var postList = _postListHandlers.GetAllPostBySaveListID(saveListID);
            if (postList == null || postList.Count == 0)
            {
                return NotFound();
            }
            return Ok(postList);
        }
        [HttpPost]
        public IActionResult AddSaveList([FromForm] int userID, [FromForm] string name)
        {
            var saveList = _saveListHandlers.AddSaveList(userID, name);
            if (saveList == null)
            {
                return BadRequest();
            }
            return Ok(saveList);
        }

        [HttpPut]
        public IActionResult UpdateSaveList([FromForm]int saveListID, [FromForm]string name)
        {
            var saveList = _saveListHandlers.UpdateSaveListName(saveListID, name);
            if (saveList == null)
            {
                return NotFound();
            }
            return Ok(saveList);
        }

        [HttpDelete("{saveListID}")]
        public IActionResult DisableSaveList(int saveListID)
        {
            var saveList = _saveListHandlers.DisableSaveList(saveListID);
            if(saveList == null)
            {
                return BadRequest();
            }
            return Ok(saveList);
        }
        [HttpDelete("posts")]
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
