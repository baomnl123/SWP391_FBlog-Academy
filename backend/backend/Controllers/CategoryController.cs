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
        private readonly IRepositoryBase<Category> _repositoryBase;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryBase<Category> repositoryBase, IMapper mapper) 
        {
           _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_repositoryBase.GetAll());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(Category categoryId)
        {
            if(!_repositoryBase.IsExists(categoryId)) return NotFound();

            var category = _mapper.Map<CategoryDTO>(_repositoryBase.GetOne(categoryId));
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(category);
        }


    }
}
