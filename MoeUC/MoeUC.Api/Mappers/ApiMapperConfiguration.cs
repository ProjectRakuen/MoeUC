using AutoMapper;
using MoeUC.Api.Models;
using MoeUC.Core.Domain;
using MoeUC.Core.Infrastructure.Mapping;

namespace MoeUC.Api.Mappers;

public class ApiMapperConfiguration : Profile, IMapperConfiguration
{
    public ApiMapperConfiguration()
    {
        CreateMap<MoeUser, UserRegisterResponseModel>();
    }

    public int Order => 999;
}