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
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public ICollection<Post>? GetAllPosts()
        {
            return _fBlogAcademyContext.Posts.Where(p => p.Status && p.IsApproved).OrderByDescending(p => p.CreatedAt).ToList();
        }

        public Post? GetPost(int postId)
        {
            return _fBlogAcademyContext.Posts.FirstOrDefault(p => p.Id == postId);
        }

        public ICollection<Post>? SearchPostsByTitle(string title)
        {
            try
            {
                var listPost = _fBlogAcademyContext.Posts.Where(p => p.Status
                                                                && p.Title.ToLower().Contains(title.ToLower())
                                                                && p.IsApproved == true).OrderByDescending(p => p.CreatedAt).ToList();
                return listPost != null ? listPost : (ICollection<Post>?)null;
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
            return _fBlogAcademyContext.Posts.Where(p => p.Status && !p.IsApproved).OrderByDescending(p => p.CreatedAt).ToList();
        }

        public ICollection<Post>? SearchPostByUserId(int userId)
        {
            try
            {
                var existedList = _fBlogAcademyContext.Posts.Where(p => p.UserId == userId
                                                                    && p.Status && p.IsApproved)
                                                                    .OrderByDescending(p => p.CreatedAt).ToList();
                return existedList != null ? (ICollection<Post>)existedList : null;
            }
            catch (InvalidOperationException)
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
                return existedList != null ? (ICollection<Post>)existedList : null;
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
                return existedList != null ? (ICollection<Post>)existedList : null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<Post>? GetPost(int[] categoryID, int[] tagID)
        {
            try
            {
                List<Post> posts = new();
                var postList = _fBlogAcademyContext.Posts.ToArray();
                //check if post has all the category and tags
                if(tagID == null || tagID.Length == 0)
                {
                    foreach (var post in postList)
                    {
                        var categories = _fBlogAcademyContext.PostCategories.OrderByDescending(p => p.Post.CreatedAt).Where(e => e.PostId.Equals(post.Id)).Select(e => e.CategoryId).ToArray();
                        if (AreAllElementsInArray(categoryID, categories))
                        {
                            posts.Add(post);
                        }
                    }
                }else if(categoryID == null || categoryID.Length == 0)
                {
                    foreach (var post in postList)
                    {
                        var tags = _fBlogAcademyContext.PostTags.OrderByDescending(p => p.Post.CreatedAt).Where(e => e.PostId.Equals(post.Id)).Select(e => e.TagId).ToArray();
                        if (AreAllElementsInArray(tagID, tags))
                        {
                            posts.Add(post);
                        }
                    }
                }
                else
                {
                    foreach (var post in postList)
                    {
                        var tags = _fBlogAcademyContext.PostTags.OrderByDescending(p => p.Post.CreatedAt).Where(e => e.PostId.Equals(post.Id)).Select(e => e.TagId).ToArray();
                        var categories = _fBlogAcademyContext.PostCategories.Where(e => e.PostId.Equals(post.Id)).Select(e => e.CategoryId).ToArray();
                        if (AreAllElementsInArray(categoryID, categories) && AreAllElementsInArray(tagID, tags))
                        {
                            posts.Add(post);
                        }
                    }
                }

                if (posts.Count == 0)
                {
                    return null;
                }
                return posts;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        static bool AreAllElementsInArray(int[] sub, int[] main)
        {
            return sub.All(item => main.Contains(item));
        }
    }
}
