using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagHandlers _tagHandlers;

        public TagController(ITagHandlers tagHandlers)
        {
            _tagHandlers = tagHandlers;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var tags = _tagHandlers.GetTags();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }
        
        [HttpGet("disable")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetDisableTags()
        {
            var tags = _tagHandlers.GetDisableTags();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(int tagId)
        {
            var tag = _tagHandlers.GetTagById(tagId);
            if (tag == null) return NotFound();

            return Ok(tag);
        }

        [HttpGet("{tagId}/post")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsByTag(int tagId)
        {
            var posts = _tagHandlers.GetPostsByTag(tagId);
            if (posts == null) return NotFound();

            return Ok(posts);
        }

        [HttpGet("{tagId}/category")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoriesByTag(int tagId)
        {
            var categories = _tagHandlers.GetCategoriesByTag(tagId);
            if (categories == null) return NotFound();

            return Ok(categories);
        }

        [HttpPost("create/{tagName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateTag([FromBody] int adminId, string tagName)
        {
            if (!_tagHandlers.CreateTag(adminId, tagName))
            {
                ModelState.AddModelError("", "Tag aldready exists!");
                return StatusCode(422, ModelState);
            }


            return Ok("Successfully create!");
        }

        [HttpPut("update/{tagName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromBody] string newTagName, string tagName)
        {
            if (!_tagHandlers.UpdateTag(tagName, newTagName))
                return NotFound();

            return Ok("Update successfully!");
        }

        [HttpPut("enable")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableTag(int tagId)
        {
            if (!_tagHandlers.EnableTag(tagId))
                ModelState.AddModelError("", "Something went wrong enable tag");

            return Ok("Enable successfully!");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int tagId)
        {
            if (!_tagHandlers.DisableTag(tagId))
                ModelState.AddModelError("", "Something went wrong deleting tag");

            return Ok("Delete successfully!");
        }
    }
}
