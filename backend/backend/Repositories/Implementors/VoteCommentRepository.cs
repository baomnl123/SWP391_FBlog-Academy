using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class VoteCommentRepository : IVoteCommentRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;

        public VoteCommentRepository()
        {
            _fBlogAcademyContext = new();
        }

        public bool Add(VoteComment comment)
        {
            try
            {
                _fBlogAcademyContext.Add(comment);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool DisableAllVoteCommentOf(Comment comment)
        {
            try
            {
                var listVote = _fBlogAcademyContext.VoteComments.Where(v => v.CommentId == comment.Id).ToList();
                foreach (var vote in listVote)
                {
                    if (vote.UpVote || vote.DownVote)
                    {
                        vote.UpVote = false;
                        vote.DownVote = false;
                        Update(vote);
                    }
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public ICollection<User>? GetAllUserBy(int commentId)
        {
            return _fBlogAcademyContext.VoteComments.Where(v => v.CommentId == commentId 
                                                            && ((v.UpVote && !v.DownVote))).Select(v => v.User).ToList();
        }

        public VoteComment? GetVoteComment(int userId, int commentId)
        {
            return _fBlogAcademyContext.VoteComments.Where(v => v.UserId == userId
                                                            && v.CommentId == commentId).FirstOrDefault();
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

        public bool Update(VoteComment comment)
        {
            try
            {
                _fBlogAcademyContext.Update(comment);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
