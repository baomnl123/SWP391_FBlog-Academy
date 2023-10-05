using AutoMapper;
using backend.DTO;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDTO>>(_tagRepository.GetTags());
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(int tagId)
        {
            if (!_tagRepository.TagExists(tagId)) return NotFound();

            var tag = _mapper.Map<TagDTO>(_tagRepository.GetTag(tagId));
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(tag);
        }

        [HttpGet("{tagId}/post")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]
        public IActionResult GetPostByTag(int tagId)
        {
            var posts = _mapper.Map<List<Post>>(_tagRepository.GetPostsByTag(tagId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromBody] TagDTO tag)
        {
            if (tag == null) return BadRequest(ModelState);

            if (_tagRepository.TagExists(tag.Id))
            {
                ModelState.AddModelError("", "Tag already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tagMap = _mapper.Map<Tag>(tag);

            if (!_tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully create!");
        }

        [HttpPut("{tagId}/update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int tagId, [FromBody] TagDTO updateTagName)
        {
            if (updateTagName == null) return BadRequest(ModelState);

            if (!_tagRepository.TagExists(tagId))
                return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var tagMap = _mapper.Map<Tag>(updateTagName);
            if (!_tagRepository.UpdateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return Ok("Update successfully!");
        }
    }
}
