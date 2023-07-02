using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Test.StartUps;

public class DbTestStartUp : IMoeTestStartup
{
    public void ConfigureService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MoeDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("MoeUC")!));
    }

    public int Order => 0;
}