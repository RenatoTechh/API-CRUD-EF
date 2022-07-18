using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;

namespace ApiCrud.Profiles
{
    public class SubcategoriaProfile : Profile
    {
        public SubcategoriaProfile()
        {
            CreateMap<CreateSubcategoriaDto, Subcategoria>();
            CreateMap<UpdateSubcategoriaDto, Subcategoria>();
            CreateMap<Subcategoria, ReadSubcategoriaDto>();
        }
    }
}
