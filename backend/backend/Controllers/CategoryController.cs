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

        [HttpPost("create-category/{adminId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateCategory(int adminId, [FromForm] string categoryName)
        {
            // If category already exists, return
            var category = _categoryHandlers.GetCategoryByName(categoryName);
            if (category.Status == true) return StatusCode(422, "Category aldready exists!");

            // If category already exists, but was disabled, then enable it
            if (category.Status == false)
            {
                _categoryHandlers.EnableCategory(category.Id);
                return Ok(category);
            }

            var createCategory = _categoryHandlers.CreateCategory(adminId, categoryName);
            if (createCategory == null) return BadRequest(ModelState);

            return Ok(createCategory);
        }

        [HttpPut("update/{currentCategoryName}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromForm] string newCategoryName, string currentCategoryName)
        {
            if (_categoryHandlers.GetCategoryByName(newCategoryName) != null)
                return StatusCode(422, "Category aldready exists!");

            var updateCategory = _categoryHandlers.UpdateCategory(currentCategoryName, newCategoryName);
            if (updateCategory == null) return BadRequest();

            return Ok(updateCategory);
        }

        [HttpPut("enable/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableCategory(int categoryId)
        {
            var enableCategory = _categoryHandlers.EnableCategory(categoryId);
            if (enableCategory == null)
                ModelState.AddModelError("", "Something went wrong enable category");

            return Ok(enableCategory);
        }

        [HttpDelete("delete/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            var deleteCategory = _categoryHandlers.DisableCategory(categoryId);
            if (deleteCategory == null)
                ModelState.AddModelError("", "Something went wrong deleting category");

            return Ok(deleteCategory);
        }
    }
}
