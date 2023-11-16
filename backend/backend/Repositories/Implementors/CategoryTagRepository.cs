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

        public bool CreateMajorSubject(MajorSubject categorySubject)
        {
            _context.Add(categorySubject);
            return Save();
        }

        public bool DisableMajorSubject(MajorSubject categorySubject)
        {
            categorySubject.Status = false;
            _context.Update(categorySubject);
            return Save();
        }

        public bool EnableMajorSubject(MajorSubject categorySubject)
        {
            categorySubject.Status = true;
            _context.Update(categorySubject);
            return Save();
        }

        public bool UpdateMajorSubject(MajorSubject categorySubject)
        {
            _context.Update(categorySubject);
            return Save();
        }

        public MajorSubject? GetMajorSubject(int tagId, int categoryId)
        {
            return _context.MajorSubjects.FirstOrDefault(c => c.SubjectId == tagId && c.MajorId == categoryId);
        }

        public ICollection<MajorSubject> GetMajorSubjectsByMajorId(int categoryId)
        {
            return _context.MajorSubjects.Where(c => c.MajorId == categoryId).ToList();
        }

        public ICollection<MajorSubject> GetMajorSubjectsBySubjectId(int tagId)
        {
            return _context.MajorSubjects.Where(c => c.SubjectId == tagId).ToList();
        }

        public ICollection<Major>? GetMajorsOf(int tagId)
        {
            List<Major> categories = new();
            var list = _context.MajorSubjects.Where(c => c.SubjectId == tagId && c.Status).Select(c => c.Major).ToList();
            if (list is null || list.Count == 0) return null;
            foreach (var item in list)
            {
                if (item.Status) categories.Add(item);
            }
            return categories.Count == 0 ? null : categories;
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
