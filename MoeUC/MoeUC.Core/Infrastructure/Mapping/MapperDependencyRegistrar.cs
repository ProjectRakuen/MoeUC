using System.NetPro;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Core.Infrastructure.Mapping;

public class MapperDependencyRegistrar : IDependencyRegistrar
{
    public void Register(IServiceCollection service, ITypeFinder typeFinder, IConfiguration configuration)
    {
        var mapperConfigurations = typeFinder.FindClassesOfType<IMapperConfiguration>();

            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IMapperConfiguration)Activator.CreateInstance(mapperConfiguration)!)
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            var config = new MapperConfiguration(config =>
            {
                foreach (var instance in instances)
                {
                    config.AddProfile(instance.GetType());
                }
            });

            config.AssertConfigurationIsValid();

            MapperHelper.Init(config);
    }

    public int Order => 0;
}