using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class CommentRepository : ICommentRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;

        public CommentRepository()
        {
            _fBlogAcademyContext = new();
        }

        public bool Add(Comment newComment)
        {
            try
            {
                _fBlogAcademyContext.Add(newComment);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public Comment? GetComment(int commentId)
        {
           return _fBlogAcademyContext.Comments.Where(c => c.Id.Equals(commentId)).FirstOrDefault();
        }

        public bool Save()
        {
            try
            {
                var saved = _fBlogAcademyContext.SaveChanges();
                return saved > 0;
            }
            catch(DbUpdateException)
            {
                //return false if there is an error when updating data
                //                  or data which is being stored is invalid
                return false;
            }
            catch(DBConcurrencyException)
            {
                //return false if stored data is changed by another user.
                //                  or there is more than one change to one object at the same time
                return false;
            }
        }

        public ICollection<Comment>? ViewAllComments(int postId)
        {
           return _fBlogAcademyContext.Comments.Where(c => c.PostId.Equals(postId) && c.Status == true)
                                                .OrderByDescending(c => c.CreatedAt).ToList();
        }
    }
}
