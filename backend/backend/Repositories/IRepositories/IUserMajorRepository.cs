using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IUserMajorRepository
    {
        public bool Add(UserMajor userMajor);
        public bool Update(UserMajor userMajor);
        public UserMajor? GetBy(int userID, int majorID);
        public ICollection<Major>? GetMajorsOf(int userID);
        public ICollection<User>? GetUsersOf(int majorID);
    }
}
