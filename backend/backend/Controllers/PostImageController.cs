using AutoMapper;
using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostImageController : Controller
    {
        private readonly IPostImageRepository _postImageRepository;
        private readonly IMapper _mapper;

        public PostImageController(IPostImageRepository postImageRepository, IMapper mapper)
        {
            _postImageRepository = postImageRepository;
            _mapper = mapper;
        }

        [HttpGet("{postId}/images")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostImage>))]
        [ProducesResponseType(400)]
        public IActionResult GetImageByPost(int postId )
        {
            var images = _mapper.Map<List<Post>>(_postImageRepository.GetImagesByPost(postId));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(images);
        }
    }
}
