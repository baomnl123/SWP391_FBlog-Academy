using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Implementors
{
    public class ReportPostRepository : IReportPostRepository
    {
        private readonly FBlogAcademyContext _fblogAcademyContext;
        private readonly ReportStatusConstrant _reportStatusConstrant;
        public ReportPostRepository()
        {
            _fblogAcademyContext = new();
            _reportStatusConstrant = new();
        }
        public bool AddReportPost(ReportPost reportpost)
        {
            try
            {
                _fblogAcademyContext.ReportPosts.Add(reportpost);
                if (_fblogAcademyContext.SaveChanges() == 0)
                {
                    return false;
                }
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool DisableReportPost(ReportPost reportpost)
        {
            //Get Report Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //
            if (!this.isReported(reportpost.ReporterId, reportpost.PostId))
            {
                return false;
            }
            else
            {
                reportpost.Status = disableStatus;
            }
            if (!this.UpdateReportPost(reportpost))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICollection<ReportPost>? GetAllReportPost()
        {
            try
            {
                var list = _fblogAcademyContext.ReportPosts.OrderBy(u => u.CreatedAt).ToList();
                if (list.Count == 0)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public ICollection<ReportPost>? GetReportPostsByContent(string content)
        {
            var list = _fblogAcademyContext.ReportPosts.Where(u => u.Content.Contains(content)).OrderBy(u => u.CreatedAt).ToList();
            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        public ReportPost? GetReportPostByID(int postID)
        {
            try
            {
                var reportPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.PostId.Equals(postID));
                return reportPost;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool UpdateReportPost(ReportPost reportpost)
        {
            try
            {
                _fblogAcademyContext.Update(reportpost);
                if (_fblogAcademyContext.SaveChanges() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool isReported(int reporterID, int postID)
        {
            try
            {
                var reportPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.ReporterId.Equals(reporterID)
                                                                                   && u.PostId.Equals(postID));
                if (reportPost == null)
                {
                    return false;
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        public ReportPost? GetReportPostByIDs(int reporterID, int postID)
        {
            try
            {
                var reportPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.ReporterId.Equals(reporterID)
                                                                               && u.PostId.Equals(postID));
                return reportPost;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

    }
}
