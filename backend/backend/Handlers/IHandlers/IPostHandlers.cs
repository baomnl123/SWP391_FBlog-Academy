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
        public PostDTO UpdatePost(int userId, string content);
        //Delete post
        public PostDTO DeletePost(int userId);
        //View pending post's list
        public ICollection<PostDTO> ViewPendingPostList();
        //Approve post
        public PostDTO ApprovePost(PostDTO post);
        //Deny post
        public PostDTO DenyPost(PostDTO post);
    }
}
