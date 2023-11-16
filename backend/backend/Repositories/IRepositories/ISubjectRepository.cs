using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface ISubjectRepository
    {
        // Get list
        public ICollection<Subject> GetAllSubjects();
        public ICollection<Subject> GetDisableSubjects();
        // Get specific
        public Subject? GetSubjectById(int subjectId);
        public Subject? GetSubjectByName(string subjectName);
        public ICollection<Post> GetPostsBySubject(int subjectId);
        public ICollection<Major> GetMajorsBySubject(int subjectId);
        // CRUD
        public bool CreateSubject(Subject subject);
        public bool UpdateSubject(Subject subject);
        public bool EnableSubject(Subject subject);
        public bool DisableSubject(Subject subject);
        public bool Save();
    }
}
