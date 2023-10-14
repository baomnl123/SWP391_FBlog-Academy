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
            CreateMap<Category, CategoryDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Video, VideoDTO>();
        }
    }
}
