using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostSubjectRepository : IPostSubjectRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostSubjectRepository()
        {
            _context = new();
        }

        public bool CreatePostSubject(PostSubject postSubject)
        {
            _context.Add(postSubject);
            return Save();
        }

        public bool DisablePostSubject(PostSubject postSubject)
        {
            postSubject.Status = false;
            _context.Update(postSubject);
            return Save();
        }

        public bool EnablePostSubject(PostSubject postSubject)
        {
            postSubject.Status = true;
            _context.Update(postSubject);
            return Save();
        }

        public bool UpdatePostSubject(PostSubject postSubject)
        {
            _context.Update(postSubject);
            return Save();
        }

        public PostSubject? GetPostSubject(int postId, int tagId)
        {
            return _context.PostSubjects.FirstOrDefault(c => c.PostId == postId && c.SubjectId == tagId);
        }

        public ICollection<PostSubject> GetPostSubjectsByPostId(int postId)
        {
            return _context.PostSubjects.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<PostSubject> GetPostSubjectsBySubjectId(int tagId)
        {
            return _context.PostSubjects.Where(c => c.SubjectId == tagId).ToList();
        }

        public ICollection<Subject>? GetSubjectsOf(int postId)
        {
            List<Subject> tags = new();
            var list = _context.PostSubjects.Where(r => r.PostId == postId && r.Status).Select(r => r.Subject).ToList();
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
