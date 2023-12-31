﻿using backend.DTO;

namespace backend.Handlers.IHandlers
{
    public interface IReportPostHandlers
    {
        public ReportPostDTO? AddReportPost(int reporterID, int postID, string content);
        public ReportPostDTO? UpdateReportPost(int reporterID, int postID, string content);
        public ReportPostDTO? ApproveReportPost(int adminID, int reporterID, int postID);
        public ICollection<ReportPostDTO>? GetAllPendingReportPost();
        public ICollection<ReportPostDTO>? GetAllPendingReportPost(int userID);
        public ICollection<ReportPostDTO>? GetAllReportPost();

        public ReportPostDTO? DenyReportPost(int adminID, int reporterID, int postID);
    }
}
