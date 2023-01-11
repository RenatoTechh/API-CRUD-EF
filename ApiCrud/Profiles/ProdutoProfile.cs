using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;

namespace ApiCrud.Profiles
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<CreateProdutoDto, Produto>();
            CreateMap<UpdateProdutoDto, Produto>();
            CreateMap<Produto, ReadProdutoDto>();
        }
    }
}
