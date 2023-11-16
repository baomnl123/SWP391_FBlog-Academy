using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IMajorRepository
    {
        // Get list
        public ICollection<Major> GetAllMajors();
        public ICollection<Major> GetDisableMajors();
        // Get specific
        public Major? GetMajorById(int majorId);
        public Major? GetMajorByName(string majorName);
        public ICollection<Post> GetPostsByMajor(int majorId);
        public ICollection<Subject> GetSubjectsByMajor(int majorId);
        // CRUD
        public bool CreateMajor(Major major);
        public bool UpdateMajor(Major major);
        public bool EnableMajor(Major major);
        public bool DisableMajor(Major major);
        public bool Save();
    }
}
