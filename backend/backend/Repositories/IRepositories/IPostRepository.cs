using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostRepository
    {
        public ICollection<Post> GetAllPosts();
        public ICollection<Post>? SearchPostsByContent(string content);
    }
}
