using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class TagRepository : ITagRepository
    {
        private readonly FBlogAcademyContext _context;

        public TagRepository()
        {
            _context = new();
        }

        public bool CreateTag(Tag tag)
        {
            _context.Add(tag);
            return Save();
        }

        public bool DisableTag(Tag tag)
        {
            tag.Status = false;
            _context.Update(tag);
            return Save();
        }

        public bool EnableTag(Tag tag)
        {
            tag.Status = true;
            _context.Update(tag);
            return Save();
        }

        public ICollection<Tag> GetAllTags()
        {
            return _context.Tags.Where(c => c.Status == true).ToList();
        }

        public ICollection<Tag> GetDisableTags()
        {
            return _context.Tags.Where(c => c.Status == false).ToList();
        }

        public ICollection<Post> GetPostsByTag(int tagId)
        {
            return _context.PostTags.Where(e => e.TagId == tagId)
                                    .Select(c => c.Post)
                                    .Where(c => c.Status == true).ToList();
        }

        public Tag? GetTagById(int tagId)
        {
            return _context.Tags.FirstOrDefault(c => c.Id == tagId);
        }

        public Tag? GetTagByName(string tagName)
        {
            return _context.Tags.FirstOrDefault(c => c.TagName == tagName);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            // if saved > 0 then return true, else return false
            return saved > 0;
        }

        public bool UpdateTag(Tag tag)
        {
            _context.Update(tag);
            return Save();
        }

        public ICollection<Category> GetCategoriesByTag(int tagId)
        {
            return _context.CategoryTags.Where(e => e.TagId == tagId)
                                        .Select(c => c.Category)
                                        .Where(c => c.Status == true).ToList();
        }
    }
}
