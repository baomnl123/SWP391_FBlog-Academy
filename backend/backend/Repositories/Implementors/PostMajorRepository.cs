using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostMajorRepository : IPostMajorRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostMajorRepository()
        {
            _context = new();
        }

        public bool CreatePostMajor(PostMajor postMajor)
        {
            _context.Add(postMajor);
            return Save();
        }

        public bool DisablePostMajor(PostMajor postMajor)
        {
            postMajor.Status = false;
            _context.Update(postMajor);
            return Save();
        }

        public bool EnablePostMajor(PostMajor postMajor)
        {
            postMajor.Status = true;
            _context.Update(postMajor);
            return Save();
        }

        public bool UpdatePostMajor(PostMajor postMajor)
        {
            _context.Update(postMajor);
            return Save();
        }

        public PostMajor? GetPostMajor(int postId, int majorId)
        {
            return _context.PostMajors.FirstOrDefault(c => c.PostId == postId && c.MajorId == majorId);
        }

        public ICollection<PostMajor> GetPostMajorsByMajorId(int majorId)
        {
            return _context.PostMajors.Where(c => c.MajorId == majorId).ToList();
        }

        public ICollection<PostMajor> GetPostMajorsByPostId(int postId)
        {
            return _context.PostMajors.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<Major>? GetMajorsOf(int postId)
        {
            List<Major> categories = new();
            var list = _context.PostMajors.Where(r => r.PostId == postId && r.Status).Select(r => r.Major).ToList();
            if(list == null || list.Count == 0) return null;
            foreach(var item in list)
            {
                if(item.Status) categories.Add(item);
            }
            return categories.Count == 0? null : categories;
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
