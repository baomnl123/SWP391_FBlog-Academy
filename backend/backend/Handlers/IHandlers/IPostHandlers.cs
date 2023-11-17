using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IPostHandlers
    {
        //Get all posts of user
        public ICollection<PostDTO>? SearchPostByUserId(int userId);
        //Search all posts
        public ICollection<PostDTO>? GetAllPosts(int currentUserId);
        //Search Posts which contain content.
        public ICollection<PostDTO>? SearchPostsByTitle(string title, int currentUserId);
        //View pending posts' list
        public ICollection<PostDTO>? ViewPendingPostList(int currentUserId);
        //View pending posts' list of particular user
        public ICollection<PostDTO>? ViewPendingPostListOf(int userId);
        public ICollection<PostDTO>? ViewDeletedPostOf(int userId);
        //Get posts by both majors and subjects
        public ICollection<PostDTO>? GetAllPosts(int[] majorIDs, int[] subjectIDs,string searchValue, int currentUserId);
        //Get post by major and tag that user follows
        public ICollection<PostDTO>? GetAllPostsOnLoad(int currentUserId);
        //Get post by post id and currentUserId
        public PostDTO? GetPostBy(int postId, int currentUserId);
        //Get post by post id
        public PostDTO? GetPostBy(int postId);
        //Get posts have Media
        public ICollection<PostDTO>? GetPostsHaveImage(int currentUserId);
        public ICollection<PostDTO>? GetPostsHaveVideo(int currentUserId);
        //Get top 5 post with highest vote.
        public ICollection<PostDTO>? GetTop5VotedPost(int currentUserId);


        //Create post
        public PostDTO? CreatePost(int userId, string title, string content, 
                                                    int[]? subjectIds, int[]? majorIds,
                                                    string[]? imageURLs, string[]? videoURLs);
        public PostDTO? CreatePost(int userId, string title, string content);
        public ICollection<SubjectDTO>? AttachSubjectsForPost(PostDTO createdPost, int[] subjectIds);
        public ICollection<MajorDTO>? AttachMajorsForPost(PostDTO createdPost, int[] majorIds);
        
        //Update post
        public PostDTO? UpdatePost(int postId, string title, string content,
                                                int[] subjectIds, int[] majorIds,
                                                string[]? imageURLs, string[]? videoURLs);
        public ICollection<ImageDTO>? UpdateImagesOfPost(int postId, string[] imageURLs);
        public ICollection<VideoDTO>? UpdateVideosOfPost(int postId, string[] videoURLs);

        //Delete post
        public PostDTO? DisablePost(int postId);
        public PostDTO? DisableAllRelatedToPost(PostDTO deletingPost);
        //Delete only post (not related data)
        public PostDTO? Delete(int postId);

        //Approve post
        public PostDTO? ApprovePost(int reviewerId, int postId);
        //Deny post
        public PostDTO? DenyPost(int reviewerId, int postId);
    }
}
