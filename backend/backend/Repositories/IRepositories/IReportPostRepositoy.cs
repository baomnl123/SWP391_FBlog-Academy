using backend.Models;

namespace backend.Repositories.IRepositories
{
    public interface IReportPostRepositoy
    {
        //Get Report Post
        public ICollection<ReportPost> GetAllReportPost();
        public ICollection<ReportPost> GetReportPostsByContent(string content);
        public ICollection<ReportPost> GetReportPostsByUser(User user);
        //CRUD Report Post
        public bool CreateReportPost(ReportPost reportpost);
        public bool UpdateReportPost(ReportPost reportpost);
        public bool DisableReportPost(ReportPost reportpost);
        //Check Existed
        public bool isExisted(ReportPost reportpost);

    }
}
