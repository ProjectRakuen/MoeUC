using System.NetPro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MoeUC.Core.Infrastructure.Dependency;

public class ScoopedDependencyRegistrar : IDependencyRegistrar
{
    public void Register(IServiceCollection service, ITypeFinder typeFinder, IConfiguration configuration)
    {
        var scoopedTypes = typeFinder.FindClassesOfType<IScoped>();

        foreach (var scoopedType in scoopedTypes)
        {
            service.AddScoped(scoopedType);
        }
    }

    public int Order => 0;
}