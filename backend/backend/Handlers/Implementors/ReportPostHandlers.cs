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
        private readonly IPostHandlers _postHandlers;

        public ReportPostHandlers(IMapper mapper,
                                  IReportPostRepository reportPostRepository,
                                  IUserRepository userRepository,
                                  IPostRepository postRepository,
                                  IPostHandlers postHandlers)
        {
            _userRoleConstrant = new();
            _reportStatusConstrant = new();
            _mapper = mapper;
            _reportPostRepository = reportPostRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postHandlers = postHandlers;
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
            //if (!reporter.Role.Contains(studentRole) && !reporter.Role.Contains(moderatorRole))
            //{
            //    return null;
            //}
            //var reported post 
            var reportedPost = _postRepository.GetPost(postID);
            if (reportedPost == null || !reportedPost.Status)
            {
                return null;
            }
            //Get Pending Status
            var pending = _reportStatusConstrant.GetPendingStatus();
            var reportPostDTO = new ReportPostDTO();
            //Check Exist
            if (_reportPostRepository.isReported(reporterID, postID))
            {
                var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
                var disableStatus = _reportStatusConstrant.GetDisableStatus();
                var declineStatus = _reportStatusConstrant.GetDeclinedStatus();
                var pendingStatus = _reportStatusConstrant.GetPendingStatus();
                //If it is available then return null
                if (!reportPost.Status.Contains(disableStatus) && !reportPost.Status.Contains(declineStatus))
                {
                    return null;
                }
                reportPost.AdminId = null;
                if (content == null)
                {
                    content = string.Empty;
                }
                reportPost.Content = content;
                reportPost.Status = pendingStatus;
                if (!_reportPostRepository.UpdateReportPost(reportPost))
                {
                    return null;
                }

                reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
                var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
                if ((getUser?.Status == true) && (getPost?.Status == true))
                {
                    reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                    reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                    if (reportPost.AdminId != null && reportPost.AdminId != 0)
                    {
                        var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                        reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                    }
                }
                return reportPostDTO;
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
            else
            {
                reportPostDTO = _mapper.Map<ReportPostDTO>(newReportPost);

                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(newReportPost.ReporterId));
                var getPost = _mapper.Map<PostDTO?>(_postRepository.GetPost(newReportPost.PostId));
                if ((getUser != null || getUser.Status) && (getPost != null || getPost.Status))
                {
                    reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                    reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                    if (newReportPost.AdminId != null && newReportPost.AdminId != 0)
                    {
                        var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(newReportPost.AdminId.HasValue ? newReportPost.AdminId.Value : 0));
                        reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                    }
                }
            }
            //Map To DB
            return reportPostDTO;
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

            var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
            var getPost = _mapper.Map<PostDTO?>(_postRepository.GetPost(reportPost.PostId));
            if ((getUser != null || getUser.Status) && (getPost != null || getPost.Status))
            {
                reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                if (reportPost.AdminId != null && reportPost.AdminId != 0)
                {
                    var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                    reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                }
            }

            return reportPostDTO;
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
                    var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
                    var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
                    if ((getUser?.Status == true) && (getPost?.Status == true))
                    {
                        var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

                        reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                        reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                        if (reportPost.AdminId != null && reportPost.AdminId != 0)
                        {
                            var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                            reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                        }
                        reportPostList.Add(reportPostDTO);
                    }
                }
            }
            if (reportPostList.Count == 0)
            {
                return null;
            }
            //Return Result
            return reportPostList;
        }

        public ICollection<ReportPostDTO>? GetAllPendingReportPost(int userID)
        {
            //get user
            var user = _userRepository.GetUser(userID);
            if (user == null || !user.Status)
            {
                return null;
            }
            //Get List
            var list = _reportPostRepository.GetAllReportPost(userID);
            //Check Null
            if (list == null || list.Count == 0)
            {
                return null;
            }
            //Init List
            var reportPostList = new List<ReportPostDTO>();
            //Get Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            foreach (var reportPost in list)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
                var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
                if ((getUser?.Status == true) && (getPost?.Status == true))
                {
                    var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

                    reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                    reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                    if (reportPost.AdminId != null && reportPost.AdminId != 0)
                    {
                        var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                        reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                    }

                    reportPostList.Add(reportPostDTO);
                }
            }
            if (reportPostList.Count == 0)
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

            var reportPostList = new List<ReportPostDTO>();

            foreach (var reportPost in list)
            {
                var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
                var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
                if ((getUser?.Status == true) && (getPost?.Status == true))
                {
                    var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

                    reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                    reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                    if (reportPost.AdminId != null && reportPost.AdminId != 0)
                    {
                        var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                        reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                    }

                    reportPostList.Add(reportPostDTO);
                }
            }

            return reportPostList;
        }

        public ReportPostDTO? UpdateReportPost(int reporterID, int postID, string content)
        {
            //get reporter
            var reporter = _userRepository.GetUser(reporterID);
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

            var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
            var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
            if ((getUser?.Status == true) && (getPost?.Status == true))
            {
                reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                if (reportPost.AdminId != null && reportPost.AdminId != 0)
                {
                    var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                    reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                }
            }

            //Map To ReportPostDTO
            return reportPostDTO;
        }

        public ReportPostDTO? ApproveReportPost(int adminID, int reporterID, int postID)
        {
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
            //Get Status
            var disableStatus = _reportStatusConstrant.GetDisableStatus();
            var approveStatus = _reportStatusConstrant.GetApprovedStatus();
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByIDs(reporterID, postID);
            //Check Null
            if (reportPost == null || reportPost.Status.Contains(disableStatus))
            {
                return null;
            }
            var postDTO = _postHandlers.GetPostBy(postID, reporterID);
            if (postDTO == null || !postDTO.Status)
            {
                return null;
            }
            if(_postHandlers.DisablePost(postDTO.Id) == null)
            {
                return null;
            }
            //Update Status
            reportPost.AdminId = adminID;
            reportPost.Status = approveStatus;
            if (!_reportPostRepository.UpdateReportPost(reportPost))
            {
                return null;
            }

            //Map To ReportPostDTO
            var reportPostDTO = _mapper.Map<ReportPostDTO>(reportPost);

            var getUser = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.ReporterId));
            var getPost = _mapper.Map<PostDTO?>(_postHandlers.GetPostBy(reportPost.PostId));
            if ((getUser?.Status == true) && (getPost?.Status == true))
            {
                reportPostDTO.Reporter = (getUser is not null && getUser.Status) ? getUser : null;
                reportPostDTO.Post = (getPost is not null && getPost.Status) ? getPost : null;
                if (reportPost.AdminId != null && reportPost.AdminId != 0)
                {
                    var getAdmin = _mapper.Map<UserDTO?>(_userRepository.GetUser(reportPost.AdminId.HasValue ? reportPost.AdminId.Value : 0));
                    reportPostDTO.Admin = (getAdmin is not null && getAdmin.Status) ? getAdmin : null;
                }
            }

            return reportPostDTO;
        }
    }
}
