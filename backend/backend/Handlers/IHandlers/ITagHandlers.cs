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
        public bool CreateTag(int adminId, string tagName, int[] categoryIds);
        public bool UpdateTag(string currentTagName, string newTagName);
        public bool EnableTag(int tagId);
        public bool DisableTag(int tagId);
    }
}
