using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class MajorSubjectRepository : IMajorSubjectRepository
    {
        private readonly FBlogAcademyContext _context;

        public MajorSubjectRepository()
        {
            _context = new();
        }

        public bool CreateMajorSubject(MajorSubject majorSubject)
        {
            _context.Add(majorSubject);
            return Save();
        }

        public bool DisableMajorSubject(MajorSubject majorSubject)
        {
            majorSubject.Status = false;
            _context.Update(majorSubject);
            return Save();
        }

        public bool EnableMajorSubject(MajorSubject majorSubject)
        {
            majorSubject.Status = true;
            _context.Update(majorSubject);
            return Save();
        }

        public bool UpdateMajorSubject(MajorSubject majorSubject)
        {
            _context.Update(majorSubject);
            return Save();
        }

        public MajorSubject? GetMajorSubject(int subjectId, int majorId)
        {
            return _context.MajorSubjects.FirstOrDefault(c => c.SubjectId == subjectId && c.MajorId == majorId);
        }

        public ICollection<MajorSubject> GetMajorSubjectsByMajorId(int majorId)
        {
            return _context.MajorSubjects.Where(c => c.MajorId == majorId).ToList();
        }

        public ICollection<MajorSubject> GetMajorSubjectsBySubjectId(int subjectId)
        {
            return _context.MajorSubjects.Where(c => c.SubjectId == subjectId).ToList();
        }

        public ICollection<Major>? GetMajorsOf(int subjectId)
        {
            List<Major> majors = new();
            var list = _context.MajorSubjects.Where(c => c.SubjectId == subjectId && c.Status).Select(c => c.Major).ToList();
            if (list is null || list.Count == 0) return null;
            foreach (var item in list)
            {
                if (item.Status) majors.Add(item);
            }
            return majors.Count == 0 ? null : majors;
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
