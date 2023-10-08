using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostHandlers : IPostHandlers
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostHandlers(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public bool ApprovePost(int postId)
        {
            throw new NotImplementedException();
        }

        public PostDTO CreatePost(int userId, string content)
        {
            throw new NotImplementedException();
        }

        public bool DeletePost(int userId, int postId)
        {
            throw new NotImplementedException();
        }

        public bool DenyPost(int postId)
        {
            throw new NotImplementedException();
        }

        public ICollection<PostDTO> GetAllPosts()
        {
            var existed = _postRepository.GetAllPosts();
            return _mapper.Map<List<PostDTO>>(existed);
        }

        public ICollection<PostDTO>? SearchPostsByContent(string content)
        {
            //init listPost to return
            List<PostDTO> listPost = new List<PostDTO>();
            
            //Search all posts which contain content
            var list = _postRepository.SearchPostsByContent(content);

            //check null
            if (list != null)
            {
                //Map to PostDTO
                listPost = _mapper.Map<List<PostDTO>>(list);
                return listPost;
            }
            return null;
        }

        public ICollection<PostDTO>? SearchPostsByMajor(string categoryName)
        {
            throw new NotImplementedException();
        }

        public ICollection<PostDTO>? SearchPostsBySubjectCode(string tagName)
        {
            throw new NotImplementedException();
        }

        public PostDTO UpdatePost(int userId, int postId, string content)
        {
            throw new NotImplementedException();
        }

        public ICollection<PostDTO> ViewPendingPostList()
        {
            throw new NotImplementedException();
        }
    }
}
