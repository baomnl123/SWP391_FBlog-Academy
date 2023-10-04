using AutoMapper;
using backend.DTO;
using backend.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<Tag, TagDTO>();

        }
    }
}
