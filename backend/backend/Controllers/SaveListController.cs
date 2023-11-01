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

        /// <summary>
        /// Get list of enable Savelist of selected User. (Student | Moderator)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of enable Savelist of selected User. (Student | Moderator)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of Posts that are saved from this Savelist. 
        /// </summary>
        /// <param name="saveListID"></param>
        /// <returns></returns>
        [HttpGet("{saveListID}/posts")]
        public IActionResult GetAllPostBySaveList(int saveListID)
        {
            var postList = _postListHandlers.GetAllPostBySaveListID(saveListID);
            if (postList == null || postList.Count == 0)
            {
                return NotFound();
            }
            return Ok(postList);
        }

        /// <summary>
        /// Create a new Savelist. (Student | Moderator)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update selected Savelist. (Student | Moderator)
        /// </summary>
        /// <param name="saveListID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("{saveListID}")]
        public IActionResult UpdateSaveList([FromForm]int saveListID, [FromForm]string name)
        {
            var saveList = _saveListHandlers.UpdateSaveListName(saveListID, name);
            if (saveList == null)
            {
                return NotFound();
            }
            return Ok(saveList);
        }

        /// <summary>
        /// Delete selected Savelist. (Student | Moderator)
        /// </summary>
        /// <param name="saveListID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete selected Post from selected Savelist. (Student | Moderator)
        /// </summary>
        /// <param name="saveListID"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        [HttpDelete("{saveListID}/{postID}")]
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
