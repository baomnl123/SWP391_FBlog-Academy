using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IUserSubjectHandlers
    {
        public UserSubjectDTO? AddUserSubject(int currentUserID, int subjectID);
        public UserSubjectDTO? DeleteUserSubject(int currentUserID, int subjectID);
        public ICollection<SubjectDTO>? GetSubjectsOf(int userID);
        public ICollection<UserDTO>? GetUsersOf(int subjectID);
    }
}
