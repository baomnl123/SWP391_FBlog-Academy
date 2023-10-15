using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public Tag? GetTagById(int tagId)
        {
            return _context.Tags.FirstOrDefault(c => c.Id == tagId);
        }

        public Tag? GetTagByName(string tagName)
        {
            return _context.Tags.FirstOrDefault(c => c.TagName.Trim().ToUpper() == tagName.Trim().ToUpper());
        }

        public ICollection<Post> GetPostsByTag(int tagId)
        {
            return _context.PostTags.Where(e => e.TagId == tagId && e.Status == true)
                                    .Select(e => e.Post)
                                    .Where(c => c.Status == true).ToList();
        }

        public ICollection<Category> GetCategoriesByTag(int tagId)
        {
            return _context.CategoryTags.Where(e => e.TagId == tagId && e.Status == true)
                                        .Select(e => e.Category)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateTag(Tag tag)
        {
            _context.Update(tag);
            return Save();
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                // if saved > 0 then return true, else return false
                return saved > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}
