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

        public bool CreateTag(int categoryId, Tag tag)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId && c.Status == true);
            if(category == null) return false;

            // Add CategoryTag
            var categoryTag = new CategoryTag()
            {
                Category = category,
                Tag = tag,
                Status = true
            };
            _context.Add(categoryTag);

            _context.Add(tag);
            return Save();
        }

        public bool DisableTag(Tag tag)
        {
            var categoryTags = _context.CategoryTags.Where(c => c.TagId == tag.Id).ToList();
            var postTags = _context.PostTags.Where(c => c.TagId == tag.Id).ToList();

            foreach (var categoryTag in categoryTags)
            {
                categoryTag.Status = false;
                _context.Update(categoryTag);
            }

            foreach (var postTag in postTags)
            {
                postTag.Status = false;
                _context.Update(postTag);
            }

            tag.Status = false;
            _context.Update(tag);
            return Save();
        }

        public bool EnableTag(Tag tag)
        {
            var categoryTags = _context.CategoryTags.Where(c => c.TagId == tag.Id).ToList();
            var postTags = _context.PostTags.Where(c => c.TagId == tag.Id).ToList();

            foreach (var categoryTag in categoryTags)
            {
                categoryTag.Status = true;
                _context.Update(categoryTag);
            }

            foreach (var postTag in postTags)
            {
                postTag.Status = true;
                _context.Update(postTag);
            }

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
