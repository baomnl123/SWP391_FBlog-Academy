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
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly ReportStatusConstrant _reportStatusConstrant;
        private readonly IUserRepository _userRepository;
        private readonly IReportPostRepository _reportPostRepository;
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;

        public ReportPostHandlers(IMapper mapper,
                                  IReportPostRepository reportPostRepository,
                                  IUserRepository userRepository,
                                  IPostRepository postRepository)
        {
            _userRoleConstrant = new();
            _reportStatusConstrant = new();
            _mapper = mapper;
            _reportPostRepository = reportPostRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }
        public ReportPostDTO? AddReportPost(int reporterID, int postID, string content)
        {
            //get reporter
            var reporter = _userRepository.GetUser(reporterID);
            //check reporter is available or not
            if (reporter == null || !reporter.Status)
            {
                return null;
            }
            //get role
            var studentRole = _userRoleConstrant.GetStudentRole();
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //check reporter is student or moderator
            if (!reporter.Role.Contains(studentRole) && !reporter.Role.Contains(moderatorRole))
            {
                return null;
            }
            //var reported post 
            var reportedPost = _postRepository.GetPost(postID);
            if (reportedPost == null || !reportedPost.Status)
            {
                return null;
            }
            //Get Pending Status
            var pending = _reportStatusConstrant.GetPendingStatus();
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
            if (content == null)
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

        public ReportPostDTO? DenyReportPost(int adminID, int reporterID, int postID)
        {
            //get admin
            var admin = _userRepository.GetUser(adminID);
            if (admin == null || !admin.Status)
            {
                return null;
            }
            //get admin role
            var adminRole = _userRoleConstrant.GetAdminRole();
            if (!admin.Role.Contains(adminRole))
            {
                return null;
            }
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            //Check Null
            if (reportPost == null || reportPost.Status.Contains(disableStatus))
            {
                return null;
            }
            //Disable Report
            if (!_reportPostRepository.DisableReportPost(reportPost))
            {
                return null;
            }
            return _mapper.Map<ReportPostDTO>(reportPost);
        }

        public ICollection<ReportPostDTO>? GetAllPendingReportPost()
        {
            //Get List
            var list = _reportPostRepository.GetAllReportPost();
            //Check Null
            if (list == null || list.Count == 0)
            {
                return null;
            }
            //Init List
            var reportPostList = new List<ReportPostDTO>();
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
            if(reportPostList.Count == 0)
            {
                return null;
            }
            //Return Result
            return reportPostList;
        }
        public ICollection<ReportPostDTO>? GetAllReportPost()
        {
            var list = _reportPostRepository.GetAllReportPost();
            if (list == null || list.Count == 0)
            {
                return null;
            }
            return _mapper.Map<List<ReportPostDTO>>(list);
        }

        public ReportPostDTO? UpdateReportPost(int reporterID, int postID, string content)
        {
            //get reporter
            var reporter = _userRepository.GetUser(reporterID);
            if(reporter == null || !reporter.Status)
            {
                return null;
            }
            //get role
            var studentRole = _userRoleConstrant.GetStudentRole();
            var moderatorRole = _userRoleConstrant.GetModeratorRole();
            //check reporter is student or moderator
            if (!reporter.Role.Contains(studentRole) && !reporter.Role.Contains(moderatorRole))
            {
                return null;
            }
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
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

        public ReportPostDTO? UpdateReportStatus(int adminID, int reporterID, int postID, string status)
        {
            //Get Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            var approveStatus = _reportStatusConstrant.GetApprovedStatus();
            var declinedStatus = _reportStatusConstrant.GetDeclinedStatus();
            var pendingStatus = _reportStatusConstrant.GetPendingStatus();
            if (!status.Contains(disableStatus)
                && !status.Contains(approveStatus)
                && !status.Contains(declinedStatus)
                && !status.Contains(pendingStatus))
            {
                return null;
            }

            //get admin
            var admin = _userRepository.GetUser(adminID);
            //check admin is available or not
            if (admin == null || !admin.Status)
            {
                return null;
            }
            //get admin role
            var adminRole = _userRoleConstrant.GetAdminRole();
            //check if it is admin or not
            if (!admin.Role.Contains(adminRole))
            {
                return null;
            }
            //get reporter
            var reporter = _userRepository.GetUser(reporterID);
            //check reporter is available or not
            if (reporter == null || !reporter.Status)
            {
                return null;
            }
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
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
