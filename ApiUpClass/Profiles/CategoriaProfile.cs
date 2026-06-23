using ApiUpClass.Dtos;
using ApiUpClass.Dtos.Responses;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<CategoriaDto, Categoria>();
            CreateMap<Categoria, CategoriaResponseDto>();
        }
    }
}
