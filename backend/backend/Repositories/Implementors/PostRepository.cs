using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostRepository : IPostRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;
        public PostRepository()
        {
            _fBlogAcademyContext = new();
        }

        public bool CreateNewPost(Post post)
        {
            try
            {
                _fBlogAcademyContext.Add(post);
                return Save();
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }

        public ICollection<Post>? GetAllPosts()
        {
            return _fBlogAcademyContext.Posts.Where(p => p.Status == true
                                                     && p.IsApproved == true).OrderByDescending(p => p.CreatedAt).ToList();
        }

        public Post? GetPost(int postId)
        {
            return _fBlogAcademyContext.Posts.FirstOrDefault(p => p.Id == postId);
        }

        public ICollection<Post>? SearchPostsByTitle(string title)
        {
            try
            {
                var listPost = _fBlogAcademyContext.Posts.Where(p => p.Status == true
                                                                && p.Title.Contains(title)
                                                                && p.IsApproved == true).OrderByDescending(p => p.CreatedAt).ToList();
                if (listPost != null)
                {
                    return listPost;
                }
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool UpdatePost(Post post)
        {
            try
            {
                _fBlogAcademyContext.Update(post);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool Save()
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

        public ICollection<Post>? ViewPendingPostList()
        {
            return _fBlogAcademyContext.Posts.Where(p => p.Status == true 
                                                    && p.IsApproved == false).OrderBy(p => p.CreatedAt).ToList();
        }

        public ICollection<Post>? SearchPostByUserId(int userId)
        {
            try
            {
                var existedList = _fBlogAcademyContext.Posts.Where(p => p.UserId == userId 
                                                                    && p.Status && p.IsApproved)
                                                                    .OrderByDescending(p => p.CreatedAt).ToList();
                if (existedList == null) return null;
                return existedList;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<Post>? ViewPendingPostList(int userId)
        {
            try
            {
                var existedList = _fBlogAcademyContext.Posts.Where(p => p.UserId == userId
                                                                    && p.Status && !p.IsApproved)
                                                                    .OrderByDescending(p => p.CreatedAt).ToList();
                if (existedList == null) return null;
                return existedList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<Post>? ViewDeletedPost(int userId)
        {
            try
            {
                var existedList = _fBlogAcademyContext.Posts.Where(p => p.UserId == userId
                                                                    && !p.Status && p.IsApproved)
                                                                    .OrderByDescending(p => p.CreatedAt).ToList();
                if (existedList == null) return null;
                return existedList;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
