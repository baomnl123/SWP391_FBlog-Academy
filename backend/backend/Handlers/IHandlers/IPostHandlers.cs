using backend.DTO;
using backend.Models;

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
        //View pending posts' list
        public ICollection<PostDTO>? ViewPendingPostList();
        //View pending posts' list of particular user
        public ICollection<PostDTO>? ViewPendingPostListOf(int userId);
        public ICollection<PostDTO>? ViewDeletedPostOf(int userId);
        //Get posts by both categories and tags
        public ICollection<PostDTO>? GetAllPosts(int[] categoryIDs, int[] tagIDs,string searchValue);
        //Get post by post id
        public PostDTO? GetPostBy(int postId);
        //Get posts have video
        public ICollection<PostDTO>? GetPostsHaveVideo();
        //Get posts have image
        public ICollection<PostDTO>? GetPostsHaveImage();

        //Create post
        public PostDTO? CreatePost(int userId, string title, string content, 
                                                    int[]? tagIds, int[]? categoryIds, 
                                                    string[]? videoURLs, string[]? imageURLs);
        public PostDTO? CreatePost(int userId, string title, string content);
        public ICollection<TagDTO>? AttachTagsForPost(PostDTO createdPost, int[] tagIds);
        public ICollection<CategoryDTO>? AttachCategoriesForPost(PostDTO createdPost, int[] categoryIds);
        
        //Update post
        public PostDTO? UpdatePost(int postId, string title, string content,
                                                int[] tagIds, int[] categoryIds,
                                                string[] videoURLs, string[] imageURLs);
        public ICollection<VideoDTO>? UpdateVideosOfPost(int postId, string[] videoURLs);
        public ICollection<ImageDTO>? UpdateImagesOfPost(int postId, string[] imageURLs);

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
