using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;

namespace backend.Repositories.Implementors
{
    public class ReportPostRepository : IReportPostRepositoy
    {
        private readonly FBlogAcademyContext _fblogAcademyContext;
        public ReportPostRepository()
        {
            _fblogAcademyContext = new();
        }
        public bool CreateReportPost(ReportPost reportpost)
        {
            _fblogAcademyContext.ReportPosts.Add(reportpost);
            if (_fblogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }

        public bool DisableReportPost(ReportPost reportpost)
        {
            ReportStatusConstrant reportStatusConstrant = new ReportStatusConstrant();
            var disableStatus = reportStatusConstrant.GetDisableStatus();
            if (!this.isExisted(reportpost)) return false;
            else
            {
                reportpost.Status = disableStatus;
            }
            if (this.UpdateReportPost(reportpost)) return true;
            else return false;
        }

        public ICollection<ReportPost> GetAllReportPost()
        {
            var list = _fblogAcademyContext.ReportPosts.ToList();
            return list;
        }

        public ICollection<ReportPost> GetReportPostsByContent(string content)
        {
            var list = _fblogAcademyContext.ReportPosts.Where(u => u.Content.Contains(content)).ToList();
            return list;
        }

        public ICollection<ReportPost> GetReportPostsByUser(User user)
        {
            var list = _fblogAcademyContext.ReportPosts.Where(u => u.ReporterId.Equals(user.Id)).ToList();
            return list;
        }

        public bool isExisted(ReportPost reportpost)
        {
            var checkPost = _fblogAcademyContext.ReportPosts.FirstOrDefault(u => u.PostId.Equals(reportpost.PostId)
                                                                              && u.ReporterId.Equals(reportpost.ReporterId));
            if (checkPost == null) return false;
            else return true;
        }

        public bool UpdateReportPost(ReportPost reportpost)
        {
            _fblogAcademyContext.Update(reportpost);
            if (_fblogAcademyContext.SaveChanges() != 0) return true;
            else return false;
        }
    }
}
