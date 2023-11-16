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

        public bool CreateMajor(Major category)
        {
            // Add Major
            _context.Add(category);
            return Save();
        }

        public bool DisableMajor(Major category)
        {
            var postMajors = _context.PostMajors.Where(c => c.MajorId == category.Id).ToList();

            foreach (var postMajor in postMajors)
            {
                postMajor.Status = false;
                _context.Update(postMajor);
            }

            category.Status = false;
            _context.Update(category);
            return Save();
        }

        public bool EnableMajor(Major category)
        {
            var categorySubjects = _context.MajorSubjects.Where(c => c.MajorId ==  category.Id).ToList();
            var postMajors = _context.PostMajors.Where(c => c.MajorId == category.Id).ToList();

            foreach (var categorySubject in categorySubjects)
            {
                categorySubject.Status = true;
                _context.Update(categorySubject);
            }

            foreach (var postMajor in postMajors)
            {
                postMajor.Status = true;
                _context.Update(postMajor);
            }

            category.Status = true;
            _context.Update(category);
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

        public Major? GetMajorById(int categoryId)
        {
            return _context.Majors.FirstOrDefault(e => e.Id == categoryId);
        }

        public Major? GetMajorByName(string categoryName)
        {
            return _context.Majors.FirstOrDefault(c => c.MajorName.Trim().ToUpper() == categoryName.Trim().ToUpper());
        }

        public ICollection<Post> GetPostsByMajor(int categoryId)
        {
            return _context.PostMajors.Where(e => e.MajorId == categoryId && e.Status == true)
                                          .Select(e => e.Post)
                                          .Where(c => c.Status == true).ToList();
        }

        public ICollection<Subject> GetSubjectsByMajor(int categoryId)
        {
            return _context.MajorSubjects.Where(e => e.MajorId == categoryId && e.Status == true)
                                        .Select(e => e.Subject)
                                        .Where(c => c.Status == true).ToList();
        }

        public bool UpdateMajor(Major category)
        {
            _context.Update(category);
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
