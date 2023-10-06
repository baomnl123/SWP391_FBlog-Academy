using backend.DTO;
using backend.Handlers.IHandlers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveListController : ControllerBase
    {
        private readonly ISaveListHandlers _saveListHandlers;
        public SaveListController(ISaveListHandlers saveListHandlers)
        {
            _saveListHandlers = saveListHandlers;
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

        [HttpPut()]
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
            if (!_saveListHandlers.DisableSaveList(saveListID))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
