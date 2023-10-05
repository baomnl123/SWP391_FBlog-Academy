using backend.DTO;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;
using System.Net.NetworkInformation;

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

        public bool DisableReportPost(ReportPost reportpost)
        {
            //Get Report Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //
            if (!this.isExisted(reportpost))
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
                var status = _reportStatusConstrant.GetDisableStatus;
                var list = _fblogAcademyContext.ReportPosts.Where(u => !u.Status.Trim().Equals(status)).ToList();
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
            var status = _reportStatusConstrant.GetDisableStatus;
            var list = _fblogAcademyContext.ReportPosts.Where(u => u.Content.Contains(content) && !u.Status.Trim().Equals(status)).ToList();
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
                var status = _reportStatusConstrant.GetDisableStatus;
                var reportPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.PostId.Equals(postID) && !u.Status.Trim().Equals(status));
                return reportPost;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool isExisted(ReportPost reportpost)
        {
            try
            {
                var status = _reportStatusConstrant.GetDisableStatus;
                var checkPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.PostId.Equals(reportpost.PostId)
                                                                              && u.ReporterId.Equals(reportpost.ReporterId)
                                                                              && !u.Status.Trim().Equals(status));
                if (checkPost == null)
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
    }
}
