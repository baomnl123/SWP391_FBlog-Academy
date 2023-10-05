using backend.DTO;
using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IReportPostRepository
    {
        //Get Report Post
        public ICollection<ReportPost>? GetAllReportPost();
        public ICollection<ReportPost>? GetReportPostsByContent(string content);
        public ReportPost? GetReportPostByID(int postID);
        //CRUD Report Post
        public bool AddReportPost(ReportPost reportpost);
        public bool UpdateReportPost(ReportPost reportpost);
        public bool DisableReportPost(ReportPost reportpost);
        //Check Existed
        public bool isExisted(ReportPost reportpost);

    }
}
