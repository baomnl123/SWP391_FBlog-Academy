using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IMajorHandlers
    {
        // Get list
        public ICollection<MajorDTO>? GetMajors();
        public ICollection<MajorDTO>? GetDisableMajors();
        // Get specific
        public MajorDTO? GetMajorById(int majorId);
        public MajorDTO? GetMajorByName(string majorName);
        public ICollection<PostDTO>? GetPostsByMajor(int majorId);
        public ICollection<SubjectDTO>? GetSubjectsByMajor(int majorId);
        public ICollection<MajorDTO>? GetTop5Majors();
        // CRUD
        public MajorDTO? CreateMajor(int adminId, string majorName);
        public MajorDTO? UpdateMajor(int currentMajorId, string newMajorName);
        public MajorDTO? EnableMajor(int majorId);
        public MajorDTO? DisableMajor(int majorId);
        public MajorDTO? CreatePostMajor(PostDTO post, MajorDTO major);
        public MajorDTO? DisablePostMajor(int postId, int majorId);
    }
}
