using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostTagRepository : IPostTagRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostTagRepository()
        {
            _context = new();
        }

        public bool CreatePostTag(PostTag postTag)
        {
            _context.Add(postTag);
            return Save();
        }

        public bool DisablePostTag(PostTag postTag)
        {
            postTag.Status = false;
            _context.Update(postTag);
            return Save();
        }

        public bool EnablePostTag(PostTag postTag)
        {
            postTag.Status = true;
            _context.Update(postTag);
            return Save();
        }

        public bool UpdatePostTag(PostTag postTag)
        {
            _context.Update(postTag);
            return Save();
        }

        public PostTag? GetPostTag(int postId, int tagId)
        {
            return _context.PostTags.FirstOrDefault(c => c.PostId == postId && c.TagId == tagId);
        }

        public ICollection<PostTag> GetPostTagsByPostId(int postId)
        {
            return _context.PostTags.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<PostTag> GetPostTagsByTagId(int tagId)
        {
            return _context.PostTags.Where(c => c.TagId == tagId).ToList();
        }

        public ICollection<Tag>? GetTagsOf(int postId)
        {
            List<Tag> tags = new();
            var list = _context.PostTags.Where(r => r.PostId == postId && r.Status).Select(r => r.Tag).ToList();
            if (list is null || list.Count == 0) return null;
            foreach (var item in list)
            {
                if (item.Status) tags.Add(item); 
            }
            return tags.Count == 0? null: tags;
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
