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

        public ICollection<PostTag> GetPostTagByPostId(int postId)
        {
            return _context.PostTags.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<PostTag> GetPostTagByTagId(int tagId)
        {
            return _context.PostTags.Where(c => c.TagId == tagId).ToList();
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
