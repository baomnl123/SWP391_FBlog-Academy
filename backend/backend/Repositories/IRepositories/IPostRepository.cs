using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostRepository
    {
        //Get Post
        public ICollection<Post>? GetAllPosts();
        public ICollection<Post>? SearchPostsByTitle(string title);
        public Post? GetPost(int postId);
        public ICollection<Post>? GetPost(int[] categoryIDs, int[] tagIDs);
        public ICollection<Post>? ViewPendingPostList();
        public ICollection<Post>? SearchPostByUserId(int userId);
        public ICollection<Post>? ViewPendingPostList(int userId);
        public ICollection<Post>? ViewDeletedPost(int userId);
        //CRUD post
        public bool CreateNewPost(Post post);
        public bool UpdatePost(Post post);
        public bool Save();
    }
}
