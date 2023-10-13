﻿using AutoMapper;
using backend.DTO;
using backend.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<ReportPost, ReportPostDTO>();
            CreateMap<SaveList, SaveListDTO>();
            CreateMap<FollowUser, FollowUserDTO>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Video, VideoDTO>();

            CreateMap<TagDTO, Tag>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Post, PostDTO>();
        }
    }
}
