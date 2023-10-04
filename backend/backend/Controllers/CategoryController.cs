using AutoMapper;
using backend.DTO;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IUserRepository userRepository, IMapper mapper) 
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId)) return NotFound();

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(categoryId));
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("{categoryId}/post")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostByCategory(int categoryId)
        {
            var posts = _mapper.Map<List<Post>>(_categoryRepository.GetPostByCategory(categoryId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromQuery] int adminId, Category category)
        {
            if (category == null) return BadRequest(ModelState);

            var isCategoryExists = _categoryRepository.CategoryExists(category.Id);
            if(isCategoryExists != null)
            {
                ModelState.AddModelError("", "Category already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);
            categoryMap.Admin = _userRepository.GetUserByID(adminId);

            if(!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully create!");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO updateCategoryName)
        {
            if (updateCategoryName == null) return BadRequest(ModelState);

            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var categoryMap = _mapper.Map<Category>(updateCategoryName);
            if(!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return Ok("Update successfully!");
        }
    }
}
