using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IPostCategoryRepository
    {
        public ICollection<PostCategory> GetPostCategoryByPostId(int postId);
        public ICollection<PostCategory> GetPostCategoryByCategoryId(int categoryId);
        public bool DisablePostCategory(PostCategory postCategory);
        public bool EnablePostCategory(PostCategory postCategory);
        public bool Save();
    }
}
