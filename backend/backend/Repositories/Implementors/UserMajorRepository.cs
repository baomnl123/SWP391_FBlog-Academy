using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Repositories.Implementors
{
    public class UserMajorRepository : IUserMajorRepository
    {
        private readonly FBlogAcademyContext _fblogAcademyContext;
        public UserMajorRepository()
        {
            _fblogAcademyContext = new();
        }
        public bool Add(UserMajor userMajor)
        {
            try
            {
                _fblogAcademyContext.UserMajors.Add(userMajor);
                return _fblogAcademyContext.SaveChanges() != 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public UserMajor? GetBy(int userID, int majorID)
        {
            try
            {
                return _fblogAcademyContext.UserMajors.Where(e => e.UserId == userID && e.MajorId == majorID).FirstOrDefault();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<Major>? GetMajorsOf(int userID)
        {
            try
            {
                return _fblogAcademyContext.UserMajors.Where(e => e.UserId == userID && e.Status == true).Select(e => e.Major).ToList();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<User>? GetUsersOf(int majorID)
        {
            try
            {
                return _fblogAcademyContext.UserMajors.Where(e => e.MajorId == majorID && e.Status == true).Select(e => e.User).ToList();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool Update(UserMajor userMajor)
        {
            try
            {
                _fblogAcademyContext.UserMajors.Update(userMajor);
                return _fblogAcademyContext.SaveChanges() != 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
