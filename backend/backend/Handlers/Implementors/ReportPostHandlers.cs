using AutoMapper;
using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Models;
using backend.Repositories.IRepositories;
using backend.Utils;

namespace backend.Handlers.Implementors
{
    public class ReportPostHandlers : IReportPostHandlers
    {
        private readonly IReportPostRepository _reportPostRepository;
        private readonly IMapper _mapper;
        private readonly ReportStatusConstrant _reportStatusConstrant;
        public ReportPostHandlers(IMapper mapper, IReportPostRepository reportPostRepository)
        {
            _reportStatusConstrant = new();
            _mapper = mapper;
            _reportPostRepository = reportPostRepository;
        }
        public ReportPostDTO? AddReportPost(int reporterID, int postID, string content)
        {
            ReportStatusConstrant reportStatusConstrant = new ReportStatusConstrant();
            //Get Pending Status
            var pending = reportStatusConstrant.GetPendingStatus();
            //Check Exist
            if (_reportPostRepository.isReported(reporterID, postID))
            {
                var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
                var disableStatus = _reportStatusConstrant.GetDisableStatus();
                var pendingStatus = _reportStatusConstrant.GetPendingStatus();
                //If it is available then return null
                if (!reportPost.Status.Contains(disableStatus))
                {
                    return null;
                }
                reportPost.AdminId = null;
                reportPost.Status = pendingStatus;
                if (!_reportPostRepository.UpdateReportPost(reportPost))
                {
                    return null;
                }
                return _mapper.Map<ReportPostDTO>(reportPost);
            }
            //Ensure that content is not null
            if(content == null)
            {
                content = string.Empty;
            }
            //Init ReportPost
            ReportPost newReportPost = new()
            {
                ReporterId = reporterID,
                PostId = postID,
                Content = content,
                Status = pending,
                CreatedAt = DateTime.Now
            };
            //Add ReportPost To DB
            if (!_reportPostRepository.AddReportPost(newReportPost))
            {
                return null;
            }
            //Map To DB
            return _mapper.Map<ReportPostDTO>(newReportPost);
        }

        public bool DenyReportPost(int reportPostID,int postID)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reportPostID,postID);
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //Check Null
            if (reportPost == null || reportPost.Status.Contains(disableStatus) )
            {
                return false;
            }
            //Disable Report
            if (!_reportPostRepository.DisableReportPost(reportPost))
            {
                return false;
            }
                return true;
        }

        public ICollection<ReportPostDTO>? GetAllPendingReportPost()
        {
            //Init List
            List<ReportPostDTO> reportPostList = new List<ReportPostDTO>();
            //Get List
            var list = _reportPostRepository.GetAllReportPost();
            //Check Null
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                //Get Status
                var pendingStatus = _reportStatusConstrant.GetPendingStatus();
                foreach (var reportPost in list)
                {
                    //Check status Status
                    if (reportPost.Status.Contains(pendingStatus))
                    {
                        //Map to ReportPostDTO
                        reportPostList.Add(_mapper.Map<ReportPostDTO>(reportPost));
                    }
                }
                //Return Result
                return reportPostList;
            }
        }
        public ICollection<ReportPostDTO>? GetAllReportPost()
        {
            var list = _reportPostRepository.GetAllReportPost();
            if(list == null || list.Count == 0)
            {
                return null;
            }
            return _mapper.Map<List<ReportPostDTO>>(list);
        }

        public ReportPostDTO? UpdateReportPost(int reportPostID, int postID, string content)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reportPostID, postID);
            //Get Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //Check Null Or Disable Status
            if (reportPost == null || reportPost.Status.Contains(disableStatus))
            {
                return null;
            }
            //Ensure that content is not null
            if (content == null)
            {
                content = string.Empty;
            }
            //Update Content
            reportPost.Content = content;
            //Update To DB
            if (!_reportPostRepository.UpdateReportPost(reportPost))
            {
                return null;
            }
                //Map To ReportPostDTO
                return _mapper.Map<ReportPostDTO>(reportPost);
        }

        public ReportPostDTO? UpdateReportStatus(int reportPostID, int postID, string status)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reportPostID, postID);
            //Get Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //Check Null
            if (reportPost == null || reportPost.Status.Contains(disableStatus))
            {
                return null;
            }
                //Update Status
                reportPost.Status = status;
                if (!_reportPostRepository.UpdateReportPost(reportPost))
                {
                    return null;
                }
                    //Map To ReportPostDTO
                    var result = _mapper.Map<ReportPostDTO>(reportPost);
                    return result;
        }
    }
}
