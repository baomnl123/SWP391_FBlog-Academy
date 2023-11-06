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
        public ICollection<TagDTO>? GetTop5Tags();
        // CRUD
        public TagDTO? CreateTag(int adminId, int categoryId, string tagName);
        public TagDTO? UpdateTag(int currentTagId, string newTagName);
        public TagDTO? EnableTag(int tagId);
        public TagDTO? DisableTag(int tagId);
        public TagDTO? CreateCategoryTag(TagDTO tag, CategoryDTO category);
        public TagDTO? DisableCategoryTag(int tagId, int categoryId);
        public TagDTO? CreatePostTag(PostDTO post, TagDTO tag);
        public TagDTO? DisablePostTag(int postId, int tagId);
    }
}
