using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryHandlers _categoryHandlers;

        public CategoryController(ICategoryHandlers categoryHandlers)
        {
            _categoryHandlers = categoryHandlers;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryHandlers.GetCategories();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("disable")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetDisableCategories()
        {
            var categories = _categoryHandlers.GetDisableCategories();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _categoryHandlers.GetCategoryById(categoryId);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet("{categoryId}/posts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostsByCategory(int categoryId)
        {
            var posts = _categoryHandlers.GetPostsByCategory(categoryId);
            if (posts == null) return NotFound();

            return Ok(posts);
        }

        [HttpGet("{categoryId}/tag")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetTagsByCategory(int categoryId)
        {
            var tags = _categoryHandlers.GetTagsByCategory(categoryId);
            if (tags == null) return NotFound();

            return Ok(tags);
        }

        [HttpPost("create/{categoryName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateCategory([FromBody] int adminId, string categoryName)
        {
            if(_categoryHandlers.GetCategoryByName(categoryName) != null)
            {
                //ModelState.AddModelError("", "Category aldready exists!");
                return StatusCode(422, "Category aldready exists!");
            }

            if (!_categoryHandlers.CreateCategory(adminId, categoryName))
                return BadRequest(ModelState);

            return Ok("Successfully create!");
        }

        [HttpPut("update/{categoryName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromBody] string newCategoryName, string currentCategoryName)
        {
            if (_categoryHandlers.GetCategoryByName(newCategoryName) != null)
            {
                //ModelState.AddModelError("", "Category aldready exists!");
                return StatusCode(422, "Category aldready exists!");
            }

            if (!_categoryHandlers.UpdateCategory(currentCategoryName, newCategoryName))
                return NotFound();

            return Ok("Update successfully!");
        }

        [HttpPut("enable")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableCategory(int categoryId)
        {
            if (!_categoryHandlers.EnableCategory(categoryId))
                ModelState.AddModelError("", "Something went wrong enable category");

            return Ok("Enable successfully!");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryHandlers.DisableCategory(categoryId))
                ModelState.AddModelError("", "Something went wrong deleting category");

            return Ok("Delete successfully!");
        }
    }
}
