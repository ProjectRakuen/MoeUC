using Hangfire;
using Hangfire.InMemory;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Api.StartUps;

public class HangfireStartup : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(c => c.UseInMemoryStorage(new InMemoryStorageOptions()
        {
            DisableJobSerialization = true
        }));

        services.AddHangfireServer();
    }

    public void Configure(IApplicationBuilder application)
    {
        application.UseHangfireDashboard();

        // Add RecurringJob Here
    }

    public int Order => 9999;
}

public class RecurringJobService
{

}