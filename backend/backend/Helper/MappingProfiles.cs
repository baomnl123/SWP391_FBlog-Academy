using AutoMapper;
using backend.DTO;
using backend.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Tag, TagDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<PostImage, PostImageDTO>();
            CreateMap<PostVideo, PostVideoDTO>();

            CreateMap<TagDTO, Tag>();
            CreateMap<CategoryDTO, Category>();

        }
    }
}
