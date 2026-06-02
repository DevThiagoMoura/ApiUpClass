using ApiUpClass.Dtos;
using ApiUpClass.Models;
using AutoMapper;

namespace ApiUpClass.Profiles
{
    public class CursoTagProfile : Profile
    {
        public CursoTagProfile()
        {
            CreateMap<CursoTagDto, CursoTag>();
        }
    }
}