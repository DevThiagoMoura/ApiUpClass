using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<TagDto, Tag>();
            CreateMap<Tag, TagResponseDto>();
        }
    }
}
