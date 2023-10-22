using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostTagRepository
    {
        public PostTag? GetPostTag(int postId, int tagId);
        public ICollection<PostTag> GetPostTagsByPostId(int postId);
        public ICollection<PostTag> GetPostTagsByTagId(int tagId);
        public bool CreatePostTag(PostTag postTag);
        public bool UpdatePostTag(PostTag postTag);
        public bool EnablePostTag(PostTag postTag);
        public bool DisablePostTag(PostTag postTag);
        public bool Save();
    }
}
