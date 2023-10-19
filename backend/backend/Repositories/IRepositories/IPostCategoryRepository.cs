using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostCategoryRepository
    {
        public ICollection<PostCategory> GetPostCategoriesByPostId(int postId);
        public ICollection<PostCategory> GetPostCategoriesByCategoryId(int categoryId);
        public bool CreatePostCategory(PostCategory postCategory);
        public bool UpdatePostCategory(PostCategory postCategory);
        public bool EnablePostCategory(PostCategory postCategory);
        public bool DisablePostCategory(PostCategory postCategory);
        public bool Save();
    }
}
