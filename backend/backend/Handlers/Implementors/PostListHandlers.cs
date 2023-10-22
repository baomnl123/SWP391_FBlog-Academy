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
        public PostListHandlers(ISaveListRepository saveListRepository, IPostRepository postRepository, IPostListRepository postListRepository,IMapper mapper) {
            _saveListRepository = saveListRepository;
            _postRepository = postRepository;
            _postListRepository = postListRepository;
            _mapper = mapper;
        }

        public PostListDTO? AddPostList(int saveListID, int postID)
        {
            //get post list
            var postList = _postListRepository.GetPostList(saveListID, postID);
            //check null
            if(postList != null)
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
            //check if savelist and post is currently active
            var saveList = _saveListRepository.GetSaveListBySaveListID(saveListID);
            var post = _postRepository.GetPost(postID);
            //check if null
            if(saveList == null || post == null)
            {
                return null;
            }
            //check if disabled
            if(!saveList.Status || !post.Status)
            {
                return null;
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
            //get postlist
            var postList = _postListRepository.GetPostList(saveListID, postID);
            //check whether it is disable then do nothing
            if (!postList.Status)
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
            //init return list
            var returnList = new List<PostDTO>();
            //get list of post
            var list = _postListRepository.GetAllPost(saveListID);
            //check if list has nothing then return null
            if(list == null || list.Count == 0)
            {
                return null;
            }
            //check if post is active then add to new list
            foreach (var post in list)
            {
                if (post.Status)
                {
                    returnList.Add(_mapper.Map<PostDTO>(post));
                }
            }
            //return
            return returnList;
        }

        public ICollection<SaveListDTO>? GetAllSaveListByPostID(int postID,int userID)
        {
            //init return list
            var returnList = new List<SaveListDTO>();
            //get all savelist
            var savelist = _postListRepository.GetAllSaveList(postID, userID);
            //check null
            if(savelist == null || savelist.Count == 0)
            {
                return null;
            }
            //check savelist is active then return
            foreach(var save in savelist)
            {
                if (save.Status)
                {
                    returnList.Add(_mapper.Map<SaveListDTO>(save));
                }
            }
            //return
            return returnList;
        }
    }
}
