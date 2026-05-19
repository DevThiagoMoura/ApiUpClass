using AutoMapper;
using ApiUpClass.Dtos;
using ApiUpClass.Models;

namespace ApiUpClass.Profiles
{
    public class ModuloProfile : Profile
    {
        public ModuloProfile()
        {
            CreateMap<ModuloDto, Modulo>();
            CreateMap<ModuloUpdateDto, Modulo>();
        }
    }
}
