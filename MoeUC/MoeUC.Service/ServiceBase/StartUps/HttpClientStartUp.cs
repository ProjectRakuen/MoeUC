using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoeUC.Core.Infrastructure.StartupConfigs;
using MoeUC.Service.Settings;

namespace MoeUC.Service.ServiceBase.StartUps;

public class HttpClientStartUp : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(HttpClientNames.Default);
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 9999;
}