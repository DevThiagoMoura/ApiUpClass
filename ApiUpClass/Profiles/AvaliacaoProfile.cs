using ApiUpClass.Dtos;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class AvaliacaoProfile : Profile
    {
        public AvaliacaoProfile()
        {
            CreateMap<AvaliacaoDto, Avaliacao>()
                .ForMember(dest => dest.CriadoEm, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<AvaliacaoUpdateDto, Avaliacao>();
        }
    }
}