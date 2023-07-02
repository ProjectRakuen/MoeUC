using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Service.ServiceBase.Caching;

public class MemoryCacheStartUp : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddSingleton<MemoryCacheManager>();
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 0;
}