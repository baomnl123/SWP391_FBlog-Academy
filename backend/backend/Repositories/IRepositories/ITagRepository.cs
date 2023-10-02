using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ITagRepository
    {
        public ICollection<Tag> GetAllTags();
        public Tag GetTagById(string id);
        public Tag GetTagByName(string name);
        public bool TagExists(int id);
        public bool CreateTag(Tag tag);
        public bool UpdateTag(Tag tag);
        public bool DeleteTag(Tag tag);
        public bool Save();
    }
}
