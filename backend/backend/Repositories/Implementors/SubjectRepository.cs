using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly FBlogAcademyContext _context;

        public SubjectRepository()
        {
            _context = new();
        }

        public bool CreateSubject(Subject tag)
        {
            _context.Add(tag);
            return Save();
        }

        public bool DisableSubject(Subject tag)
        {
            var postSubjects = _context.PostSubjects.Where(c => c.SubjectId == tag.Id).ToList();

            foreach (var postSubject in postSubjects)
            {
                postSubject.Status = false;
                _context.Update(postSubject);
            }

            tag.Status = false;
            _context.Update(tag);
            return Save();
        }

        public bool EnableSubject(Subject tag)
        {
            var postSubjects = _context.PostSubjects.Where(c => c.SubjectId == tag.Id).ToList();

            foreach (var postSubject in postSubjects)
            {
                postSubject.Status = true;
                _context.Update(postSubject);
            }

            tag.Status = true;
            _context.Update(tag);
            return Save();
        }

        public ICollection<Subject> GetAllSubjects()
        {
            return _context.Subjects.Where(c => c.Status == true).ToList();
        }

        public ICollection<Subject> GetDisableSubjects()
        {
            return _context.Subjects.Where(c => c.Status == false).ToList();
        }

        public Subject? GetSubjectById(int tagId)
        {
            return _context.Subjects.FirstOrDefault(c => c.Id == tagId);
        }

        public Subject? GetSubjectByName(string tagName)
        {
            return _context.Subjects.FirstOrDefault(c => c.SubjectName.Trim().ToUpper() == tagName.Trim().ToUpper());
        }

        public ICollection<Post> GetPostsBySubject(int tagId)
        {
            return _context.PostSubjects.Where(e => e.SubjectId == tagId && e.Status == true)
                                    .Select(e => e.Post)
                                    .Where(c => c.Status == true).ToList();
        }

        public ICollection<Major> GetMajorsBySubject(int tagId)
        {
            return _context.MajorSubjects.Where(e => e.SubjectId == tagId && e.Status == true)
                                        .Select(e => e.Major)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateSubject(Subject tag)
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
