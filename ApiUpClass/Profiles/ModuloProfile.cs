using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class ModuloProfile : Profile
    {
        public ModuloProfile()
        {
            CreateMap<ModuloDto, Modulo>();
            CreateMap<ModuloUpdateDto, Modulo>();
            CreateMap<Modulo, ModuloResponseDto>()
                .ForMember(dest => dest.Curso, opt => opt.MapFrom(src => src.curso));
            CreateMap<Modulo, ModuloResumoResponseDto>();
        }
    }
}
