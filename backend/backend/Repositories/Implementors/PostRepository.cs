using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class PostRepository : IPostRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        public PostRepository()
        {
            _fBlogAcademyContext = new();
        }

        public ICollection<Post>? SearchPostsByContent(string content)
        {
            try
            {
                var listPost = _fBlogAcademyContext.Posts.Where(p => p.Content.Contains(content)).OrderByDescending(p => p.CreatedAt).ToList();
                if (listPost.Any())
                {
                    return listPost;
                }
                return null;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }
    }
}
