using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(string id);
        ICollection<Post> GetPostByTag(string tagId);
        bool TagExists(string id);
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
        bool Save();
    }
}
