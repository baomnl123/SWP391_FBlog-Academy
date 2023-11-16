using AutoMapper;
using backend.DTO;
using backend.Models;

namespace backend.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserDTO>();
            CreateMap<Post, PostDTO>();
            CreateMap<ReportPost, ReportPostDTO>();
            CreateMap<SaveList,SaveListDTO>();
            CreateMap<FollowUser, FollowUserDTO>();
            CreateMap<Major, MajorDTO>();
            CreateMap<Subject, SubjectDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Video, VideoDTO>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<VoteComment, VoteCommentDTO>();
            CreateMap<VotePost, VotePostDTO>();
            CreateMap<PostList,PostListDTO>();
        }
    }
}
