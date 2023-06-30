using Microsoft.EntityFrameworkCore;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Api.StartUps;

public class DbStartup : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDbContext<MoeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MoeUC")));
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 0;
}