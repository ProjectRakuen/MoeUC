using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MoeUC.Core.Infrastructure.Mapping;

public static class MapperExtensions 
{
    public static TDestination MapTo<TDestination>(this object source)
    {
        return MapperHelper.Mapper.Map<TDestination>(source);
    }

    public static TDestination MapTo<TSource,TDestination>(this TSource source, TDestination destination)
    {
        return MapperHelper.Mapper.Map(source, destination);
    }
}