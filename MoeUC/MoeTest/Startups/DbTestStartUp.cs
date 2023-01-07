using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoeUC.Core.Infrastructure.Data;

namespace MoeUC.Test.StartUps;

public class DbTestStartUp : IMoeTestStartup
{
    public void ConfigureService(IServiceCollection services, IConfiguration configuration)
    {
        var dbms = configuration.GetSection("DBMS").Value;
        if (!string.IsNullOrWhiteSpace(dbms))
        {
            if (dbms.Equals("SQLServer", StringComparison.InvariantCultureIgnoreCase))
                services.AddDbContext<MoeDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MoeUC")));
            if (dbms.Equals("MySQL", StringComparison.InvariantCultureIgnoreCase))
                services.AddDbContext<MoeDbContext>(options =>
                    options.UseMySQL(configuration.GetConnectionString("MoeUC")));
        }
        else
            throw new Exception("Must specify DBMS name.");
    }

    public int Order => 0;
}