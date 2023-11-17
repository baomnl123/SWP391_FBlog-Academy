using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class UserSubjectRepository : IUserSubjectRepository
    {
        private readonly FBlogAcademyContext _fblogAcademyContext;
        public UserSubjectRepository()
        {
            _fblogAcademyContext = new();
        }
        public bool Add(UserSubject userSubject)
        {
            try
            {
                _fblogAcademyContext.Add(userSubject);
                return _fblogAcademyContext.SaveChanges() != 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public UserSubject? GetBy(int userID, int subjectID)
        {
            try
            {
                return _fblogAcademyContext.UserSubjects.Where(e => e.UserId == userID && e.SubjectId == subjectID).FirstOrDefault();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool Update(UserSubject userSubject)
        {
            try
            {
                _fblogAcademyContext.Update(userSubject);
                return _fblogAcademyContext.SaveChanges() != 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        ICollection<Subject>? IUserSubjectRepository.GetSubjectsOf(int userID)
        {
            try
            {
                return _fblogAcademyContext.UserSubjects.Where(e => e.UserId == userID).Select(e => e.Subject).ToList();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        ICollection<User>? IUserSubjectRepository.GetUsersOf(int subjectID)
        {
            try
            {
                return _fblogAcademyContext.UserSubjects.Where(e => e.SubjectId == subjectID).Select(e => e.User).ToList();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
