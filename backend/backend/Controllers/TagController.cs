using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Handlers.Implementors;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagHandlers _tagHandlers;
        private readonly ICategoryHandlers _categoryHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly UserRoleConstrant _userRoleConstrant;

        public TagController(ITagHandlers tagHandlers, ICategoryHandlers categoryHandlers, IUserHandlers userHandlers)
        {
            _tagHandlers = tagHandlers;
            _categoryHandlers = categoryHandlers;
            _userHandlers = userHandlers;
            _userRoleConstrant = new();
        }

        /// <summary>
        /// Get list of enable Tags. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var tags = _tagHandlers.GetTags();

            if (tags == null)
            {
                var emptyList = new List<TagDTO>();
                return Ok(emptyList);
            }

            return Ok(tags);
        }

        /// <summary>
        /// Get list of enable Tags. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("disable")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetDisableTags()
        {
            var tags = _tagHandlers.GetDisableTags();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }

        /// <summary>
        /// Get selected Tag by ID.
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(int tagId)
        {
            var tag = _tagHandlers.GetTagById(tagId);
            if (tag == null || tag.Status == false) return NotFound();

            return Ok(tag);
        }

        /// <summary>
        /// Get list of Posts that contains selected Tag. (Admin)
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsByTag(int tagId)
        {
            var posts = _tagHandlers.GetPostsByTag(tagId);
            if (posts == null)
            {
                var emptyList = new List<PostDTO>();
                return Ok(emptyList);
            }

            return Ok(posts);
        }

        /// <summary>
        /// Get list of categories that contains selected Tag.
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}/categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoriesByTag(int tagId)
        {
            var categories = _tagHandlers.GetCategoriesByTag(tagId);
            if (categories == null)
            {
                var emptyList = new List<CategoryDTO>();
                return Ok(emptyList);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Create a new Tag. (Admin)
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateTag(int adminId, int categoryId, [FromForm] string tagName)
        {
            // Check if admin exists
            var admin = _userHandlers.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (admin == null || admin.Role != adminRole)
                return NotFound("Admin does not exists!");

            // Check if category does not exists
            var category = _categoryHandlers.GetCategoryById(categoryId);
            if (category == null || !category.Status) return NotFound("Category does not exists!");

            var isTagExists = _tagHandlers.GetTagByName(tagName);
            if (isTagExists != null && isTagExists.Status)
            {
                if (_tagHandlers.CreateCategoryTag(isTagExists, category) != null)
                    return Ok(isTagExists);

                return StatusCode(422, $"\"{isTagExists.TagName}\" already exists!");
            }

            if (isTagExists != null && !isTagExists.Status)
            {
                // If the tag was disabled, enable it then create relationship with Category
                _tagHandlers.EnableTag(isTagExists.Id);
                if (_tagHandlers.CreateCategoryTag(isTagExists, category) != null)
                    return Ok(isTagExists);

                return StatusCode(422, $"\"{isTagExists.TagName}\" already exists!");
            }

            // If tag is null, then create new tag
            var createTag = _tagHandlers.CreateTag(adminId, categoryId, tagName);
            if (createTag == null) return BadRequest(ModelState);

            // If create succeed, then create relationship
            _tagHandlers.CreateCategoryTag(createTag, category);

            return Ok(createTag);
        }

        /// <summary>
        /// Update selected Tag. (Admin)
        /// </summary>
        /// <param name="newTagName"></param>
        /// <param name="currentTagId"></param>
        /// <returns></returns>
        [HttpPut("{currentTagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag([FromForm] string newTagName, int currentTagId)
        {
            // If tag does not exists for updating, return Not found
            var currentTag = _tagHandlers.GetTagById(currentTagId);
            if (currentTag == null || !currentTag.Status) return NotFound("Tag does not exists!");

            // Check the new tag name already exists in DB
            var isTagExists = _tagHandlers.GetTagByName(newTagName);
            if (isTagExists != null && isTagExists.Status)
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

        /// <summary>
        /// Enable selected Tag. (Admin)
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete selected Tag. (Admin)
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpDelete("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int tagId)
        {
            var deleteTag = _tagHandlers.DisableTag(tagId);
            if (deleteTag == null) ModelState.AddModelError("", "Something went wrong delete tag");

            return Ok(deleteTag);
        }

        /// <summary>
        /// Remove selected Tag from selected Category. (Admin)
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId}/{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRelationship(int categoryId, int tagId)
        {
            var deleteRelationship = _tagHandlers.DisableCategoryTag(tagId, categoryId);
            if (deleteRelationship == null) ModelState.AddModelError("", "Something went wrong delete relationship");

            return Ok(deleteRelationship);
        }
    }
}
