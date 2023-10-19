using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ITagRepository
    {
        // Get list
        public ICollection<Tag> GetAllTags();
        public ICollection<Tag> GetDisableTags();
        // Get specific
        public Tag? GetTagById(int tagId);
        public Tag? GetTagByName(string tagName);
        public ICollection<Post> GetPostsByTag(int tagId);
        public ICollection<Category> GetCategoriesByTag(int tagId);
        // CRUD
        public bool CreateTag(Tag tag);
        public bool UpdateTag(Tag tag);
        public bool EnableTag(Tag tag);
        public bool DisableTag(Tag tag);
        public bool Save();
    }
}
