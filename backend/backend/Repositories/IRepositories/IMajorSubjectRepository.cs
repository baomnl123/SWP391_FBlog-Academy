using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IMajorSubjectRepository
    {
        public MajorSubject? GetMajorSubject(int subjectId, int majorId);
        public ICollection<MajorSubject> GetMajorSubjectsBySubjectId(int subjectId);
        public ICollection<MajorSubject> GetMajorSubjectsByMajorId(int majorId);
        public ICollection<Major>? GetMajorsOf(int subjectId);
        public bool CreateMajorSubject(MajorSubject majorSubject);
        public bool UpdateMajorSubject(MajorSubject majorSubject);
        public bool EnableMajorSubject(MajorSubject majorSubject);
        public bool DisableMajorSubject(MajorSubject majorSubject);
        public bool Save();
    }
}
