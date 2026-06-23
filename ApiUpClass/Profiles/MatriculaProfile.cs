using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class MatriculaProfile : Profile
    {
        public MatriculaProfile()
        {
            CreateMap<MatriculaDto, Matricula>()
                .ForMember(dest => dest.DataMatricula, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "ativo"));

            CreateMap<MatriculaUpdateDto, Matricula>();
            CreateMap<Matricula, MatriculaResponseDto>();
        }
    }
}
