using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IPostHandlers
    {
        //Get all posts of user
        public ICollection<PostDTO>? SearchPostByUserId(int userId);
        //Search all posts
        public ICollection<PostDTO>? GetAllPosts();
        //Search Posts which contain content.
        public ICollection<PostDTO>? SearchPostsByTitle(string title);
        //Create post
        public PostDTO? CreatePost(int userId, string title, string content, int tagId, int categoryId);
        //Update post
        public PostDTO? UpdatePost(int postId, string title, string content, int tagId, int categoryId);
        //Delete post
        public PostDTO? DeletePost(int postId, int tagId, int categoryId);
        //View pending post's list
        public ICollection<PostDTO>? ViewPendingPostList(int viewerId);
        //Approve post
        public PostDTO? ApprovePost(int reviewerId, int postId);
        //Deny post
        public PostDTO? DenyPost(int reviewerId, int postId, int tagId, int categoryId);
    }
}
