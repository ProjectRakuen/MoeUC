using System.NetPro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MoeUC.Core.Infrastructure.Dependency;

public class DiDependencyRegistrar : IDependencyRegistrar
{
    public void Register(IServiceCollection service, ITypeFinder typeFinder, IConfiguration configuration)
    {
        var singletonTypes = typeFinder.FindClassesOfType<ISingleton>().ToList();
        foreach (var singletonType in singletonTypes)
        {
            service.AddSingleton(singletonType);
        }

        var scoopedTypes = typeFinder.FindClassesOfType<IScoped>().ToList();
        foreach (var scoopedType in scoopedTypes)
        {
            service.AddScoped(scoopedType);
        }
    }

    public int Order => 0;
}