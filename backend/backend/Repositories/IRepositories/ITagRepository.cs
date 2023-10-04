using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ITagRepository
    {
        public ICollection<Tag> GetTags();
        public Tag GetTag(int tagId);
        public ICollection<Post> GetPostByTag(int tagId);
        public bool TagExists(string name);
        public bool CreateTag(Tag tag);
        public bool UpdateTag(Tag tag);
        public bool DeleteTag(Tag tag);
        public bool Save();
    }
}
