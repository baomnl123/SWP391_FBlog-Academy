using backend.DTO;
using System.Transactions;

namespace backend.Handlers.IHandlers
{
    public interface IReportPostHandlers
    {
        public ReportPostDTO? AddReportPost(int reporterID, int postID, string content);
        public ReportPostDTO? UpdateReportPost(int reportPostID, string content);
        public ReportPostDTO? UpdateReportPostStatus(int reportPostID, string status);
        public ICollection<ReportPostDTO>? GetAllPendingReportPost();
        public bool DenyReportPost(int reportPostID);
    }
}
