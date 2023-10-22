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

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateTag([FromForm] int adminId, [FromForm] int categoryId, [FromForm] string tagName)
        {
            // If tag already exists but was disabled, then enable it
            var tag = _tagHandlers.GetTagByName(tagName);
            if (tag != null && tag.Status == false)
            {
                if(tag.Status) return StatusCode(422, $"\"{tag.TagName}\" aldready exists!");

                _tagHandlers.EnableTag(tag.Id);
                return Ok(tag);
            }

            // if tag is null, then create new tag
            var createTag = _tagHandlers.CreateTag(adminId, categoryId, tagName);
            if (createTag == null) return BadRequest(ModelState);

            // If create succeed, then create relationship
            _tagHandlers.CreateRelationship(createTag.Id, categoryId);

            return Ok(createTag);
        }

        [HttpPut("update/{currentTagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag([FromForm] string newTagName, int currentTagId)
        {
            // If tag does not exists for updating, return Not found
            var currentTag = _tagHandlers.GetTagById(currentTagId);
            if(currentTag == null || !currentTag.Status) return NotFound("Tag does not exists!");

            // Check the new tag name already exists in DB
            var isTagExists = _tagHandlers.GetTagByName(newTagName);
            if(isTagExists != null && isTagExists.Status)
                return StatusCode(422, $"\"{isTagExists.TagName}\" aldready exists!");

            // If tag already exists, but was disabled, then enable it
            if (isTagExists != null && !isTagExists.Status)
            {
                _tagHandlers.EnableTag(isTagExists.Id);
                return StatusCode(422, $"\"{isTagExists.TagName}\" aldready exists!");
            }

            // If the new name does not exists, then update to the current tag
            var updateTage = _tagHandlers.UpdateTag(currentTagId, newTagName);
            if (updateTage == null) return BadRequest();

            return Ok(updateTage);
        }

        [HttpPut("enable/{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableTag(int tagId)
        {
            var enableTag = _tagHandlers.EnableTag(tagId);
            if (enableTag == null) ModelState.AddModelError("", "Something went wrong enable tag");

            return Ok(enableTag);
        }

        [HttpDelete("delete/{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int tagId)
        {
            var deleteTag = _tagHandlers.DisableTag(tagId);
            if (deleteTag == null) ModelState.AddModelError("", "Something went wrong delete tag");

            return Ok(deleteTag);
        }

        [HttpDelete("delete-relationship/{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRelationship([FromForm] int categoryId, [FromForm] int tagId)
        {
            var deleteRelationship = _tagHandlers.DisableRelationship(tagId, categoryId);
            if (deleteRelationship == null) ModelState.AddModelError("", "Something went wrong delete relationship");

            return Ok(deleteRelationship);
        }
    }
}
