using ApiUpClass.Dtos;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class CursoProfile : Profile
    {
        public CursoProfile()
        {
            CreateMap<CursoDto, Curso>()
                .ForMember(
                    dest => dest.Ativo,
                    opt => opt.MapFrom(src => true)
                )
                .ForMember(
                    dest => dest.CriadoEm,
                    opt => opt.MapFrom(src => DateTime.UtcNow)
                );

            CreateMap<CursoUpdateDto, Curso>();
        }
    }
}
