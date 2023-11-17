using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IUserSubjectRepository
    {
        public bool Add(UserSubject userSubject);
        public bool Update(UserSubject userSubject);
        public UserSubject? GetBy(int userID, int subjectID);
        public ICollection<Subject>? GetSubjectsOf(int userID);
        public ICollection<User>? GetUsersOf(int subjectID);
    }
}
