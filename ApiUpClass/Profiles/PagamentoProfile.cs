using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<PagamentoDto, Pagamento>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "pendente"))
                .ForMember(dest => dest.CriadoEm, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<PagamentoUpdateDto, Pagamento>();
            CreateMap<Pagamento, PagamentoResponseDto>();
        }
    }
}
