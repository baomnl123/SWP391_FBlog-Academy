using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IPostListHandlers
    {
        public ICollection<PostDTO>? GetAllPostBySaveListID(int saveListID);
        public ICollection<SaveListDTO>? GetAllSaveListByPostID(int postID,int userID);
        public PostListDTO? AddPostList(int saveListID, int postID);
        public PostListDTO? DisablePostList(int saveListID, int postID);
    }
}
