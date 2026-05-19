using AutoMapper;
using ApiUpClass.Dtos;
using ApiUpClass.Models;
namespace ApiUpClass.Profiles
{
    public class AulaProfile : Profile
    {
        public AulaProfile()
        {
            CreateMap<AulaDto, Aula>();
            CreateMap<AulaUpdateDto, Aula>();
        }
    }
}
