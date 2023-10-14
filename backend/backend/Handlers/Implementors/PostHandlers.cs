using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;

namespace backend.Handlers.Implementors
{
    public class PostHandlers : IPostHandlers
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostHandlers(IPostRepository postRepository, IMapper mapper, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public PostDTO? ApprovePost(int reviewerId, int postId)
        {
            //check reviewer is not null
            //                  and not yet removed
            //                  and has role MOD(Moderator) or LT(Lecturer)
            var reviewer = _userRepository.GetUser(reviewerId);
            if (reviewer != null 
                && reviewer.Status == true 
                && ( reviewer.Role.Contains("MOD") || reviewer.Role.Contains("LT") ))
            {
                //check post needed approved exists
                var existedPost = _postRepository.GetPost(postId);

                //check post is null or removed or approved
                if (existedPost == null 
                    || existedPost.Status == false
                    || ( existedPost.Status == true && existedPost.IsApproved == true ) ) return null;

                //update info of existedPost
                existedPost.ReviewerId = reviewerId;
                existedPost.IsApproved = true;
                existedPost.UpdatedAt = DateTime.Now;

                //Update info to database
                if(!_postRepository.UpdatePost(existedPost)) return null;
                return _mapper.Map<PostDTO>(existedPost);
            }

            //return null if reviewer is invalid
            return null;
        }

        public PostDTO? CreatePost(int userId, string title, string content)
        {
            //check info is not null
            if(title is null || content is null)
            {
                return null;
            }

            //check if userId is not existed
            //                or removed
            //                or user with that id does not have role SU(Student) or role MOD(Moderator)
            var existedUser = _userRepository.GetUser(userId);
            if (existedUser  == null 
                || existedUser.Status == false 
                || !( existedUser.Role.Contains("SU") || existedUser.Role.Contains("MOD") ) ) return null;

            //create new user
            Post newPost = new()
            {
                UserId = userId,
                Title = title,
                Content = content,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                Status = true,
            };

            //add new user to database
            if(!_postRepository.CreateNewPost(newPost)) return null;
            return _mapper.Map<PostDTO>(newPost);
        }

        public PostDTO? DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public PostDTO? DenyPost(int reviewerId, int postId)
        {
            //return null if validReviewer is null
            //                              or removed
            //                              or does not have role MOD(Moderator) or LT(Lecturer)
            var validReviewer = _userRepository.GetUser(reviewerId);
            if (validReviewer == null
                || validReviewer.Status == false
                || !( validReviewer.Role.Contains("MOD") || validReviewer.Role.Contains("LT") )) return null;

            //return null if existedPost is null
            //                              or removed
            //                              or approved
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null 
                ||  existedPost.Status == false 
                || ( existedPost.Status == true && existedPost.IsApproved == true ) ) return null;

            //update info of existedPost which is denied
            existedPost.ReviewerId = reviewerId;
            existedPost.IsApproved = false;
            existedPost.Status = false;
            existedPost.UpdatedAt = DateTime.Now;

            //update info to database
            if(!_postRepository.UpdatePost(existedPost)) return null;
            return _mapper.Map<PostDTO>(existedPost);
        }

        public ICollection<PostDTO>? GetAllPosts()
        {
            var existed = _postRepository.GetAllPosts();
            return _mapper.Map<List<PostDTO>>(existed);
        }

        public ICollection<PostDTO>? SearchPostByUserId(int userId)
        {
            //check that user is not null
            //                  or removed
            //                  or that user does not have role SU(Student) or role MOD(Moderator)
            var exitedUser = _userRepository.GetUser(userId);
            if (exitedUser == null 
                || exitedUser.Status == false 
                || !( exitedUser.Role.Contains("SU") || exitedUser.Role.Contains("MOD") )) return null;

            //Search Posts' list of userId
            var existedPostList = _postRepository.SearchPostByUserId(userId);

            //check null
            if (existedPostList == null) return null;

            //return posts' list
            return _mapper.Map<List<PostDTO>>(existedPostList);
        }

        public ICollection<PostDTO>? SearchPostsByTitle(string title)
        {
            //init listPost to return
            List<PostDTO> listPost = new List<PostDTO>();
            
            //Search all posts which contain content
            var list = _postRepository.SearchPostsByTitle(title);

            //check null
            if (list != null)
            {
                //Map to List<PostDTO>
                listPost = _mapper.Map<List<PostDTO>>(list);
                return listPost;
            }
            return null;
        }

        public PostDTO? UpdatePost(int postId, string title, string content)
        {
            //check post is existed
            var existedPost = _postRepository.GetPost(postId);
            if (existedPost == null || !(existedPost.Status == true && existedPost.IsApproved == true)) return null;

            //Update info of existed post
            existedPost.Title = title;
            existedPost.Content = content;
            existedPost.UpdatedAt = DateTime.Now;

            //Update to database
            if(!_postRepository.UpdatePost(existedPost)) return null;
            return _mapper.Map<PostDTO>(existedPost);
        }

        public ICollection<PostDTO>? ViewPendingPostList(int viewerId)
        {
            //return null if viewer does not exist
            //                      or is removed
            //                      or does not have role LT(Lecturer) or MOD(Moderator)
            var validViewer = _userRepository.GetUser(viewerId);
            if (validViewer == null 
                || validViewer.Status == false 
                || !( validViewer.Role.Contains("LT") || validViewer.Role.Contains("MOD") )) return null;
            //Get pending posts list
            var existedList = _postRepository.ViewPendingPostList();

            //check null
            if (existedList == null) return null;

            //Map to List<PostDTO>
            return _mapper.Map<List<PostDTO>>(existedList);
        }
    }
}
