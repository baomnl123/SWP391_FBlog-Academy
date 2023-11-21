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

        public bool CreateSubject(Subject subject)
        {
            _context.Add(subject);
            return Save();
        }

        public bool DisableSubject(Subject subject)
        {
            var postSubjects = _context.PostSubjects.Where(c => c.SubjectId == subject.Id).ToList();

            foreach (var postSubject in postSubjects)
            {
                postSubject.Status = false;
                _context.Update(postSubject);
            }

            subject.Status = false;
            _context.Update(subject);
            return Save();
        }

        public bool EnableSubject(Subject subject)
        {
            var postSubjects = _context.PostSubjects.Where(c => c.SubjectId == subject.Id).ToList();

            foreach (var postSubject in postSubjects)
            {
                postSubject.Status = true;
                _context.Update(postSubject);
            }

            subject.Status = true;
            _context.Update(subject);
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

        public Subject? GetSubjectById(int subjectId)
        {
            return _context.Subjects.FirstOrDefault(c => c.Id == subjectId);
        }

        public Subject? GetSubjectByName(string subjectName)
        {
            return _context.Subjects.FirstOrDefault(c => c.SubjectName.Trim().ToUpper() == subjectName.Trim().ToUpper());
        }

        public ICollection<Post> GetPostsBySubject(int subjectId)
        {
            return _context.PostSubjects.Where(e => e.SubjectId == subjectId && e.Status == true)
                                    .Select(e => e.Post)
                                    .Where(c => c.Status == true).ToList();
        }

        public ICollection<Major> GetMajorsBySubject(int subjectId)
        {
            return _context.MajorSubjects.Where(e => e.SubjectId == subjectId && e.Status == true)
                                        .Select(e => e.Major)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateSubject(Subject subject)
        {
            _context.Update(subject);
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
