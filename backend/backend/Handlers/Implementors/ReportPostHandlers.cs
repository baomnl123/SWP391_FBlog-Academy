using backend.DTO;
using backend.Handlers.IHandlers;

namespace backend.Handlers.Implementors
{
    public class ReportPostHandlers : IReportPostHandlers
    {
        public ReportPostDTO CreateReportPost(string reporterID, string postID, string content)
        {
            throw new NotImplementedException();
        }

        public bool DenyReportPost(int reportPostID)
        {
            throw new NotImplementedException();
        }

        public ReportPostDTO GetAllReportPost()
        {
            throw new NotImplementedException();
        }

        public ReportPostDTO UpdateReportPost(int reportPostID)
        {
            throw new NotImplementedException();
        }

        public ReportPostDTO UpdateReportPostStatus(int reportPostID)
        {
            throw new NotImplementedException();
        }
    }
}
