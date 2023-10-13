using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostTagRepository
    {
        public ICollection<PostTag> GetPostTagByPostId(int postId);
        public ICollection<PostTag> GetPostTagByTagId(int tagId);
        public bool DisablePostTag(PostTag postTag);
        public bool EnablePostTag(PostTag postTag);
        public bool Save();
    }
}
