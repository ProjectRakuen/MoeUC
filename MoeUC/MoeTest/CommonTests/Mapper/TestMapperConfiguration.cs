using AutoMapper;
using MoeUC.Core.Infrastructure.Mapping;
using MoeUC.Test.CommonTests.Mocks;

namespace MoeUC.Test.CommonTests.Mapper;

public class TestMapperConfiguration : Profile,IMapperConfiguration
{
    public int Order => 999;

    public TestMapperConfiguration()
    {
        CreateMap<MockMapSource, MockMapDestination>();
    }
}