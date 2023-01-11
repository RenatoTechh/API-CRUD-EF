using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using System.Linq;

namespace ApiCrud.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<CreateCategoriaDto, Categoria>();
            CreateMap<UpdateCategoriaDto, Categoria>();
            CreateMap<Categoria, ReadCategoriaDto>();
        }
    }
}
