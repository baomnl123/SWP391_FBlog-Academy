using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class TagRepository : ITagRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;

        public TagRepository(FBlogAcademyContext fBlogAcademyContext)
        {
            _fBlogAcademyContext = fBlogAcademyContext;
        }

        public bool CreateTag(Tag tag)
        {
            _fBlogAcademyContext.Add(tag);
            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            _fBlogAcademyContext.Remove(tag);
            return Save();
        }

        public ICollection<Tag> GetAllTags()
        {
            return _fBlogAcademyContext.Tags.ToList();
        }

        public Tag GetTagById(string id)
        {
            return _fBlogAcademyContext.Tags.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public Tag GetTagByName(string name)
        {
            return _fBlogAcademyContext.Tags.Where(c => c.TagName.Equals(name)).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _fBlogAcademyContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TagExists(int id)
        {
            return _fBlogAcademyContext.Tags.Any(c => c.Id.Equals(id));
        }

        public bool UpdateTag(Tag tag)
        {
            _fBlogAcademyContext.Update(tag);
            return Save();
        }
    }
}
