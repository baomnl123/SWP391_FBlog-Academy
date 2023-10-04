using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class TagRepository : ITagRepository
    {
        private readonly FBlogAcademyContext _context;

        public TagRepository(FBlogAcademyContext context)
        {
            _context = context;
        }

        public bool CreateTag(Tag tag)
        {
            _context.Add(tag);
            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            _context.Remove(tag);
            return Save();
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.Where(c => c.Status.Equals(true)).ToList();
        }

        public ICollection<Post> GetPostByTag(int tagId)
        {
            return _context.PostTags.Where(e => e.PostId == tagId)
                                    .Select(c => c.Post)
                                    .Where(c => c.Status.Equals(true)).ToList();
        }

        public Tag GetTag(int tagId)
        {
            return _context.Tags.Where(c => c.Id == tagId && c.Status.Equals(true)).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TagExists(string name)
        {
            return _context.Tags.Any(c => c.TagName.Trim().ToUpper() == name.Trim().ToUpper()
                                     && c.Status.Equals(true));
        }

        public bool UpdateTag(Tag tag)
        {
            _context.Update(tag);
            return Save();
        }
    }
}
