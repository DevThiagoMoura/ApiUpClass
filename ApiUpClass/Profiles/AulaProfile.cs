using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class AulaProfile : Profile
    {
        public AulaProfile()
        {
            CreateMap<AulaDto, Aula>();
            CreateMap<AulaUpdateDto, Aula>();
            CreateMap<Aula, AulaResponseDto>();
        }
    }
}
