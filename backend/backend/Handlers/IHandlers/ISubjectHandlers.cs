using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface ISubjectHandlers
    {
        // Get list
        public ICollection<SubjectDTO> GetSubjects();
        public ICollection<SubjectDTO> GetDisableSubjects();
        // Get specific
        public SubjectDTO? GetSubjectById(int subjectId);
        public SubjectDTO? GetSubjectByName(string subjectName);
        public ICollection<PostDTO>? GetPostsBySubject(int subjectId);
        public ICollection<MajorDTO>? GetMajorsBySubject(int subjectId);
        public ICollection<SubjectDTO>? GetTop5Subjects();
        // CRUD
        public SubjectDTO? CreateSubject(int adminId, int majorId, string subjectName);
        public SubjectDTO? UpdateSubject(int currentSubjectId, string newSubjectName);
        public SubjectDTO? EnableSubject(int subjectId);
        public SubjectDTO? DisableSubject(int subjectId);
        public SubjectDTO? CreateMajorSubject(SubjectDTO subject, MajorDTO major);
        public SubjectDTO? DisableMajorSubject(int subjectId, int majorId);
        public SubjectDTO? CreatePostSubject(PostDTO post, SubjectDTO subject);
        public SubjectDTO? DisablePostSubject(int postId, int subjectId);
    }
}
