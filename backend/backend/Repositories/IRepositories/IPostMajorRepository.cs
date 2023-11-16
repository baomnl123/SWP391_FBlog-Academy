using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostMajorRepository
    {
        public PostMajor? GetPostMajor(int postId, int majorId);
        public ICollection<PostMajor> GetPostMajorsByPostId(int postId);
        public ICollection<PostMajor> GetPostMajorsByMajorId(int majorId);
        public ICollection<Major>? GetMajorsOf(int postId);
        public bool CreatePostMajor(PostMajor postMajor);
        public bool UpdatePostMajor(PostMajor postMajor);
        public bool EnablePostMajor(PostMajor postMajor);
        public bool DisablePostMajor(PostMajor postMajor);
        public bool Save();
    }
}
