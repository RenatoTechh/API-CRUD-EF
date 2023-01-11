using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;

namespace ApiCrud.Profiles
{
    public class CentroDistribuicaoProfile : Profile
    {
        public CentroDistribuicaoProfile()
        {
            CreateMap<CreateCentroDistribuicaoDto, CentroDistribuicao>();
            CreateMap<UpdateCentroDistribuicaoDto, CentroDistribuicao>();
            CreateMap<CentroDistribuicao, ReadCentroDistribuicaoDto>();
        }
    }
}
