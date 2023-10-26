using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostListHandlers : IPostListHandlers
    {
        private readonly ISaveListRepository _saveListRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostListRepository _postListRepository;
        private readonly IMapper _mapper;
        public PostListHandlers(ISaveListRepository saveListRepository, IPostRepository postRepository, IPostListRepository postListRepository, IMapper mapper)
        {
            _saveListRepository = saveListRepository;
            _postRepository = postRepository;
            _postListRepository = postListRepository;
            _mapper = mapper;
        }

        public PostListDTO? AddPostList(int saveListID, int postID)
        {
            //get savelist
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);
            //check savelist is available or not
            if (saveList == null || !saveList.Status)
            {
                return null;
            } 
            //get postID 
            var post = _postRepository.GetPost(postID);
            if(post == null || !post.Status)
            {
                return null;
            }
            //get post list
            var postList = _postListRepository.GetPostList(saveListID, postID);
            //check null
            if (postList != null)
            {
                //check whether it is active then do nothing
                if (postList.Status)
                {
                    return null;
                }
                //update status and datetime
                postList.Status = true;
                postList.CreatedAt = DateTime.Now;
                //update into db
                if (!_postListRepository.UpdatePostList(postList))
                {
                    return null;
                }
                //return
                return _mapper.Map<PostListDTO>(postList);
            }
            //init new postlist
            var newPostList = new PostList()
            {
                SaveListId = saveListID,
                SavePostId = postID,
                CreatedAt = DateTime.Now,
                Status = true,
            };
            //add to db
            if (!_postListRepository.AddPostList(newPostList))
            {
                return null;
            }
            //return
            return _mapper.Map<PostListDTO>(newPostList);
        }

        public PostListDTO? DisablePostList(int saveListID, int postID)
        {
            //get savelist
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);
            //check savelist is available or not
            if (saveList == null || !saveList.Status)
            {
                return null;
            }
            //get postID 
            var post = _postRepository.GetPost(postID);
            //check post is available or not
            if (post == null || !post.Status)
            {
                return null;
            }
            //get postlist
            var postList = _postListRepository.GetPostList(saveListID, postID);
            //check whether it is disable then do nothing
            if (postList == null || !postList.Status)
            {
                return null;
            }
            //update postlist status
            postList.Status = false;
            //update to db
            if (!_postListRepository.UpdatePostList(postList))
            {
                return null;
            }
            //return
            return _mapper.Map<PostListDTO>(postList);
        }

        public ICollection<PostDTO>? GetAllPostBySaveListID(int saveListID)
        {
            //get savelist
            var savelist = _saveListRepository.GetSaveListBySaveListID(saveListID);
            //check savelist is available or not
            if (savelist == null || !savelist.Status)
            {
                return null;
            }
            //get list of post
            var posts = _postListRepository.GetAllPost(saveListID);
            //check if list has nothing then return null
            if (posts == null || posts.Count == 0)
            {
                return null;
            }
            //init return list
            var returnList = new List<PostDTO>();
            //check if post is active then add to new list
            foreach (var post in posts)
            {
                //if post is available
                if (post.Status)
                {
                    //check postlist is available or not
                    var postlist = _postListRepository.GetPostList(saveListID, post.Id);
                    if (postlist != null && postlist.Status)
                    {
                        returnList.Add(_mapper.Map<PostDTO>(post));
                    }
                }
            }
            if (returnList.Count == 0) return null;
            //return
            return returnList;
        }

        public ICollection<SaveListDTO>? GetAllSaveListByPostID(int postID, int userID)
        {
            //check post
            var post = _postRepository.GetPost(postID);
            //check available
            if (post == null || !post.Status)
            {
                return null;
            }
            //get all savelist
            var savelist = _postListRepository.GetAllSaveList(postID, userID);
            //check null
            if (savelist == null || savelist.Count == 0)
            {
                return null;
            }
            //init return list
            var returnList = new List<SaveListDTO>();
            //check savelist is active then return
            foreach (var save in savelist)
            {
                if (save.Status)
                {
                    var postlist = _postListRepository.GetPostList(save.Id, postID);
                    //check available
                    if (postlist != null && postlist.Status)
                    {
                        returnList.Add(_mapper.Map<SaveListDTO>(save));
                    }
                }
            }
            //check empty
            if (returnList.Count == 0) return null;
            //return
            return returnList;
        }
    }
}
