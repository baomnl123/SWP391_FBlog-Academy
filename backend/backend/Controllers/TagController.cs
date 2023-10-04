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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IUserRepository userRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDTO>>(_tagRepository.GetTags);
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
            var posts = _mapper.Map<List<Post>>(_tagRepository.GetPostByTag(tagId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromQuery] int adminId, Tag tag)
        {
            if (tag == null) return BadRequest(ModelState);

            var isTagExists = _tagRepository.TagExists(tag.Id);
            if (isTagExists != null)
            {
                ModelState.AddModelError("", "Tag already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tagMap = _mapper.Map<Tag>(tag);
            tagMap.Admin = _userRepository.GetUserByID(adminId);

            if (!_tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully create!");
        }

        [HttpPut("{tagId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory([FromQuery] int adminId, Tag tag, [FromBody] string updateTagName)
        {
            if (updateTagName == null) return BadRequest(ModelState);

            if (!_tagRepository.TagExists(tag.Id))
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
