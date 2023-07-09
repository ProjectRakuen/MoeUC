using AutoMapper;

namespace MoeUC.Core.Infrastructure.Mapping;

public class MapperHelper
{
    public static IMapper Mapper { get; private set; } = null!;
    public static MapperConfiguration MapperConfiguration { get; private set; } = null!;

    public static void Init(MapperConfiguration config)
    {
        MapperConfiguration = config;
        Mapper = config.CreateMapper();
    }
}