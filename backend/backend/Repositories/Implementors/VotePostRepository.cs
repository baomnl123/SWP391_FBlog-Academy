using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class VotePostRepository : IVotePostRepository
    {
        private readonly FBlogAcademyContext _fBlogAcademyContext;

        public VotePostRepository()
        {
            _fBlogAcademyContext = new();
        }
        public bool Add(VotePost newVotePost)
        {
            try
            {
                _fBlogAcademyContext.Add(newVotePost);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool DisableAllVotePostOf(Post post)
        {
            try
            {
                var voteList = _fBlogAcademyContext.VotePosts.Where(v => v.PostId == post.Id).ToList();
                foreach (var vote in voteList)
                {
                    if (vote.UpVote || vote.DownVote)
                    {
                        vote.UpVote = false;
                        vote.DownVote = false;
                    }
                }
                return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }

        public ICollection<User>? GetAllUsersVotedBy(int postId)
        {
            return _fBlogAcademyContext.VotePosts.Where(v => v.PostId == postId 
                                                            && (v.UpVote && !v.DownVote)).Select(v => v.User).ToList();
        }

        public ICollection<User>? GetAllUsersDownVotedBy(int postId)
        {
            return _fBlogAcademyContext.VotePosts.Where(v => v.PostId == postId
                                                            && (!v.UpVote && v.DownVote)).Select(v => v.User).ToList();
        }

        public VotePost? GetVotePost(int currentUserId, int postId)
        {
            return _fBlogAcademyContext.VotePosts.Where(v => v.UserId == currentUserId && v.PostId == postId).FirstOrDefault();
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
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }

        public bool Update(VotePost votePost)
        {
            try
            {
                _fBlogAcademyContext.Update(votePost);
                return Save();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
