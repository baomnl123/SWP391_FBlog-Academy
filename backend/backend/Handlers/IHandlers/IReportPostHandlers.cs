using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IReportPostHandlers
    {
        public ReportPostDTO? AddReportPost(int reporterID, int postID, string content);
        public ReportPostDTO? UpdateReportPost(int reportPostID, int postID, string content);
        public ReportPostDTO? UpdateReportStatus(int reportPostID, int postID, string status);
        public ICollection<ReportPostDTO>? GetAllPendingReportPost();
        public ICollection<ReportPostDTO>? GetAllReportPost();
        public bool DenyReportPost(int reportPostID, int postID);
    }
}
