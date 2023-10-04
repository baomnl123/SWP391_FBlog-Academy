using backend.DTO;
using System.Transactions;

namespace backend.Handlers.IHandlers
{
    public interface IReportPostHandlers
    {
        public ReportPostDTO CreateReportPost(string reporterID, string postID, string content);
        public ReportPostDTO UpdateReportPost(int reportPostID);
        public ReportPostDTO UpdateReportPostStatus(int reportPostID);
        public ReportPostDTO GetAllReportPost();
        public bool DenyReportPost(int reportPostID);
    }
}
