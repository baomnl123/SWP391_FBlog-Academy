using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostListRepository : IPostListRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        public PostListRepository()
        {
            _fBlogAcademyContext = new();
        }
        public bool AddPostList(PostList postList)
        {
            try
            {
                _fBlogAcademyContext.Add(postList);
                return IsSaved();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public ICollection<Post>? GetAllPost(int saveListID)
        {
            try
            {
                var list = _fBlogAcademyContext.PostLists.Where(e => e.SaveList.Id.Equals(saveListID))
                                                     .Select(e => e.SavePost).ToList();
                return list;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool UpdatePostList(PostList postList)
        {
            try
            {
                _fBlogAcademyContext.Update(postList);
                return IsSaved();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
            public bool IsSaved()
            {
                try
                {
                    var saved = _fBlogAcademyContext.SaveChanges();
                    return saved > 0;
                }
                catch (DbUpdateException)
                {
                    //return false if there is an error when updating data
                    //                  or data which is being stored is invalid
                    return false;
                }
                catch (DBConcurrencyException)
                {
                    //return false if stored data is changed by another user.
                    //                  or there is more than one change to one object at the same time
                    return false;
                }
            }

        public PostList? GetPostList(int saveListID, int postID)
        {
            try
            {
                var postList = _fBlogAcademyContext.PostLists.FirstOrDefault(e => e.SaveListId.Equals(saveListID) && e.SavePostId.Equals(postID));
                return postList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<SaveList>? GetAllSaveList(int postID, int userID)
        {
            try
            {
                var saveList = _fBlogAcademyContext.PostLists.Where(e => e.SavePostId.Equals(postID) && e.SaveList.UserId.Equals(userID))
                                                             .Select(e => e.SaveList).ToList();
                return saveList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
    }
