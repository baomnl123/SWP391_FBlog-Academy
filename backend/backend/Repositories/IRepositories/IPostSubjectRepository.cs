using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostSubjectRepository
    {
        public PostSubject? GetPostSubject(int postId, int subjectId);
        public ICollection<PostSubject> GetPostSubjectsByPostId(int postId);
        public ICollection<PostSubject> GetPostSubjectsBySubjectId(int subjectId);
        public ICollection<Subject>? GetSubjectsOf(int postId);
        public bool CreatePostSubject(PostSubject postSubject);
        public bool UpdatePostSubject(PostSubject postSubject);
        public bool EnablePostSubject(PostSubject postSubject);
        public bool DisablePostSubject(PostSubject postSubject);
        public bool Save();
    }
}
