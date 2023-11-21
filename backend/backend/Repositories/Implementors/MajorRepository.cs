using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class MajorRepository : IMajorRepository
    {
        private readonly FBlogAcademyContext _context;

        public MajorRepository()
        {
            _context = new();
        }

        public bool CreateMajor(Major major)
        {
            // Add Major
            _context.Add(major);
            return Save();
        }

        public bool DisableMajor(Major major)
        {
            var postMajors = _context.PostMajors.Where(c => c.MajorId == major.Id).ToList();

            foreach (var postMajor in postMajors)
            {
                postMajor.Status = false;
                _context.Update(postMajor);
            }

            major.Status = false;
            _context.Update(major);
            return Save();
        }

        public bool EnableMajor(Major major)
        {
            var majorSubjects = _context.MajorSubjects.Where(c => c.MajorId ==  major.Id).ToList();
            var postMajors = _context.PostMajors.Where(c => c.MajorId == major.Id).ToList();

            foreach (var majorSubject in majorSubjects)
            {
                majorSubject.Status = true;
                _context.Update(majorSubject);
            }

            foreach (var postMajor in postMajors)
            {
                postMajor.Status = true;
                _context.Update(postMajor);
            }

            major.Status = true;
            _context.Update(major);
            return Save();
        }

        public ICollection<Major> GetAllMajors()
        {
            return _context.Majors.Where(c => c.Status == true).ToList();
        }

        public ICollection<Major> GetDisableMajors()
        {
            return _context.Majors.Where(c => c.Status == false).ToList();
        }

        public Major? GetMajorById(int majorId)
        {
            return _context.Majors.FirstOrDefault(e => e.Id == majorId);
        }

        public Major? GetMajorByName(string majorName)
        {
            return _context.Majors.FirstOrDefault(c => c.MajorName.Trim().ToUpper() == majorName.Trim().ToUpper());
        }

        public ICollection<Post> GetPostsByMajor(int majorId)
        {
            return _context.PostMajors.Where(e => e.MajorId == majorId && e.Status == true)
                                          .Select(e => e.Post)
                                          .Where(c => c.Status == true).ToList();
        }

        public ICollection<Subject> GetSubjectsByMajor(int majorId)
        {
            return _context.MajorSubjects.Where(e => e.MajorId == majorId && e.Status == true)
                                        .Select(e => e.Subject)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateMajor(Major major)
        {
            _context.Update(major);
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
