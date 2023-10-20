using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ITagHandlers
    {
        // Get list
        public ICollection<TagDTO> GetTags();
        public ICollection<TagDTO> GetDisableTags();
        // Get specific
        public TagDTO? GetTagById(int tagId);
        public TagDTO? GetTagByName(string tagName);
        public ICollection<PostDTO>? GetPostsByTag(int tagId);
        public ICollection<CategoryDTO>? GetCategoriesByTag(int tagId);
        // CRUD
        public TagDTO? CreateTag(int adminId, int categoryId, string tagName);
        public TagDTO? UpdateTag(string currentTagName, string newTagName);
        public TagDTO? EnableTag(int tagId);
        public TagDTO? DisableTag(int tagId);
        public TagDTO? CreateRelationship(int tagId, int categoryId);
        public TagDTO? DisableRelationship(int tagId, int categoryId);
    }
}
