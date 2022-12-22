using System.NetPro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moe.Core.Infrastructure.Dependency;

namespace Moe.Core.Infrastructure.Data;

public class DataDependencyRegistrar : IDependencyRegistrar
{
    public void Register(IServiceCollection service, ITypeFinder typeFinder, IConfiguration configuration)
    {
        service.AddScoped(typeof(IRepository<,>), typeof(EntityFrameworkCoreRepository<,>));
        service.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkCoreRepository<>));
    }

    public int Order => 0;
}