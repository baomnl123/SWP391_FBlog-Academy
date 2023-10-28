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
    public class CategoryController : Controller
    {
        private readonly ICategoryHandlers _categoryHandlers;
        private readonly IUserHandlers _userHandlers;
        private readonly UserRoleConstrant _userRoleConstrant;

        public CategoryController(ICategoryHandlers categoryHandlers, IUserHandlers userHandlers)
        {
            _categoryHandlers = categoryHandlers;
            _userHandlers = userHandlers;
            _userRoleConstrant = new();
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
            if (category == null || category.Status == false) return NotFound();

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

        [HttpPost("create-category")]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateCategory(int adminId, [FromForm] string categoryName)
        {
            var admin = _userHandlers.GetUser(adminId);
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (admin == null || admin.Role != adminRole)
                return NotFound("Admin does not exists!");

            var category = _categoryHandlers.GetCategoryByName(categoryName);
            if (category != null)
            {
                // If category already exists, and status is true, then return 
                if (category.Status) return StatusCode(422, $"\"{category.CategoryName}\" aldready exists!");

                // If category already exists, but was disabled, then enable it
                return Ok(_categoryHandlers.EnableCategory(category.Id));
            }

            var createCategory = _categoryHandlers.CreateCategory(adminId, categoryName);
            if (createCategory == null) return BadRequest(ModelState);

            return Ok(createCategory);
        }

        [HttpPut("update/{currentCategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromForm] string newCategoryName, int currentCategoryId)
        {
            // If category does not exists for updating, return Not found
            var currentCategory = _categoryHandlers.GetCategoryById(currentCategoryId);
            if (currentCategory == null) return NotFound("Category does not exists!");

            // Check the new category name already exists in DB
            var isCategoryExists = _categoryHandlers.GetCategoryByName(newCategoryName);
            if (isCategoryExists != null && isCategoryExists.Status)
                return StatusCode(422, $"\"{isCategoryExists.CategoryName}\" aldready exists!");

            // If category already exists, but was disabled, then enable it
            if (isCategoryExists != null && !isCategoryExists.Status)
            {
                _categoryHandlers.EnableCategory(isCategoryExists.Id);
                return StatusCode(422, $"\"{isCategoryExists.CategoryName}\" aldready exists!");
            }

            // If the new name does not exists, then update to the current category    
            var updateCategory = _categoryHandlers.UpdateCategory(currentCategoryId, newCategoryName);
            if (updateCategory == null) return BadRequest();

            return Ok(updateCategory);
        }

        [HttpPut("enable/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult EnableCategory(int categoryId)
        {
            var category = _categoryHandlers.GetCategoryById(categoryId);
            if (category == null) return NotFound("Category does not exists!");

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
            var category = _categoryHandlers.GetCategoryById(categoryId);
            if (category == null) return NotFound("Category does not exists!");

            var deleteCategory = _categoryHandlers.DisableCategory(categoryId);
            if (deleteCategory == null)
                ModelState.AddModelError("", "Something went wrong deleting category");

            return Ok(deleteCategory);
        }
    }
}
