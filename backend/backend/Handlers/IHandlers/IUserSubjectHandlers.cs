using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IUserSubjectHandlers
    {
        public ICollection<UserSubjectDTO>? AddUserSubject(int currentUserID, int[] subjectID);
        public ICollection<UserSubjectDTO>? DeleteUserSubject(int currentUserID, int[] subjectID);
        public ICollection<SubjectDTO>? GetSubjectsOf(int userID);
        public ICollection<UserDTO>? GetUsersOf(int subjectID);
    }
}
