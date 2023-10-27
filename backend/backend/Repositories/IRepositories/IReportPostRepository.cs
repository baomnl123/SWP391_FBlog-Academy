using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IReportPostRepository
    {
        //Get Report Post
        public ICollection<ReportPost>? GetAllReportPost();
        public ICollection<ReportPost>? GetAllReportPost(int userID);
        public ICollection<ReportPost>? GetReportPostsByContent(string content);
        //public ReportPost? GetReportPostByID(int postID);
        public ReportPost? GetReportPostByIDs(int reporterID, int postID);
        //CRUD Report Post
        public bool AddReportPost(ReportPost reportpost);
        public bool UpdateReportPost(ReportPost reportpost);
        public bool DisableReportPost(ReportPost reportpost);
        //Check Existed
        public bool isReported(int reporterID, int postID);
    }
}
