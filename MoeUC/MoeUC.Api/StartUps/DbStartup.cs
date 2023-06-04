using Microsoft.EntityFrameworkCore;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Api.StartUps;

public class DbStartup : IMoeStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var dbms = configuration.GetSection("DBMS").Value;
        if (!string.IsNullOrWhiteSpace(dbms))
        {
            if (dbms.Equals("SQLServer", StringComparison.InvariantCultureIgnoreCase))
                services.AddDbContext<MoeDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MoeUC")));
            if (dbms.Equals("MySQL", StringComparison.InvariantCultureIgnoreCase))
                services.AddDbContext<MoeDbContext>(options =>
                    options.UseMySQL(configuration.GetConnectionString("MoeUC")!));
        }
        else
            throw new Exception("Must specify DBMS name.");


    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 0;
}