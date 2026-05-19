using ApiUpClass.Dtos;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.MapFrom(src => src.Senha))
                .ForMember(dest => dest.CriadoEm, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UsuarioUpdateDto, Usuario>();
        }
    }
}