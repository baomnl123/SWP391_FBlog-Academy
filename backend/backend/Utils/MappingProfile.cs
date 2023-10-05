﻿using AutoMapper;
using backend.DTO;
using backend.Models;

namespace backend.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<User, UserDTO>();
            CreateMap<ReportPost, ReportPostDTO>();
            CreateMap<SaveList,SaveListDTO>();
            CreateMap<FollowUser, FollowUserDTO>();
        }
    }
}
