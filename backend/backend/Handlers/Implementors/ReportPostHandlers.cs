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
            //Get Status
            var pending = reportStatusConstrant.GetPendingStatus();
            //Init ReportPost
            ReportPost reportPost = new()
            {
                ReporterId = reporterID,
                PostId = postID,
                AdminId = 0,
                Content = content,
                Status = pending,
                CreatedAt = DateTime.Now
            };
            //Check Exist
            if (_reportPostRepository.isExisted(reportPost))
            {
                return null;
            }
            else
            {
                //Add ReportPost To DB
                if (!_reportPostRepository.AddReportPost(reportPost))
                {
                    return null;
                }
                else
                {
                    //Map To DB
                    var result = _mapper.Map<ReportPostDTO>(reportPost);
                    return result;
                }
            }

        }

        public bool DenyReportPost(int reportPostID)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByID(reportPostID);
            //Check Null
            if (reportPost == null)
            {
                return false;
            }
            //Disable Report
            else if (!_reportPostRepository.DisableReportPost(reportPost))
            {
                return false;
            }
            else
            {
                return true;
            }
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
                var pending = _reportStatusConstrant.GetPendingStatus();
                foreach (var reportPost in list)
                {
                    //Update Status
                    if (reportPost.Status.Contains(pending))
                    {
                        //Map to ReportPostDTO
                        reportPostList.Add(_mapper.Map<ReportPostDTO>(reportPost));
                    }
                }
                //Return Result
                return reportPostList;
            }
        }

        public ReportPostDTO? UpdateReportPost(int reportPostID, string content)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByID(reportPostID);
            //Check Null
            if (reportPost == null)
            {
                return null;
            } 
            else
            {
                //Update Content
                reportPost.Content = content;
                //Update To DB
                if (!_reportPostRepository.UpdateReportPost(reportPost))
                {
                    return null;
                }
                else
                {
                    //Map To ReportPostDTO
                    var result = _mapper.Map<ReportPostDTO>(reportPost);
                    return result;
                }
            }
        }

        public ReportPostDTO? UpdateReportPostStatus(int reportPostID, string status)
        {
            //Get Report
            var reportPost = _reportPostRepository.GetReportPostByID(reportPostID);
            //Check Null
            if (reportPost == null)
            {
                return null;
            }
            else
            {
                //Update Status
                reportPost.Status = status;
                if (!_reportPostRepository.UpdateReportPost(reportPost)) {
                    return null;
                }
                else
                {
                    //Map To ReportPostDTO
                    var result = _mapper.Map<ReportPostDTO>(reportPost);
                    return result;
                }
            }
        }
    }
}
