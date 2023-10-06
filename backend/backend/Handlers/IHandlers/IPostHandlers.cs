using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IPostHandlers
    {
        //Search Posts which contain content.
        public ICollection<PostDTO> SearchPostsByContent(string content);
        //Search Posts belong to a subject code
        public ICollection<PostDTO> SearchPostsBySubjectCode(string tagName);
        //Search Posts belong to a major
        public ICollection<PostDTO> SearchPostsByMajor(string categoryName);
        //Create post
        public PostDTO CreatePost(int userId, string content);
        //Update post
        public PostDTO UpdatePost(int userId, int postId, string content);
        //Delete post
        public bool DeletePost(int userId, int postId);
        //View pending post's list
        public ICollection<PostDTO> ViewPendingPostList();
        //Approve post
        public bool ApprovePost(int postId);
        //Deny post
        public bool DenyPost(int postId);
    }
}
