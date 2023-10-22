using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostListRepository
    {
        public ICollection<Post>? GetAllPost(int saveListID);
        public ICollection<SaveList>? GetAllSaveList(int postID, int userID);
        public PostList? GetPostList(int saveListID, int postID);
        public bool AddPostList(PostList postList);
        public bool UpdatePostList(PostList postList);
        public bool IsSaved();
    }
}
