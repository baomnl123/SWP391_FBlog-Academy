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
        public TagDTO? UpdateTag(int currentTagId, string newTagName);
        public TagDTO? EnableTag(int tagId);
        public TagDTO? DisableTag(int tagId);
        public CategoryTagDTO? CreateCategoryTag(TagDTO tag, CategoryDTO category);
        public CategoryTagDTO? DisableCategoryTag(int tagId, int categoryId);
        public PostTagDTO? CreatePostTag(PostDTO post, TagDTO tag);
        public PostTagDTO? DisablePostTag(int postId, int tagId);
    }
}
