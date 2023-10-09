using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ITagRepository
    {
        public ICollection<Tag> GetTags();
        public Tag GetTag(int tagId);
        public ICollection<Post> GetPostsByTag(int tagId);
        public bool TagExists(int tagId);
        public bool CreateTag(Tag tag);
        public bool UpdateTag(Tag tag);
        public bool DeleteTag(Tag tag);
        public bool Save();
    }
}
