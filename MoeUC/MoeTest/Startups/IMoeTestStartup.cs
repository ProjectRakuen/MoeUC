using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MoeUC.Test.StartUps;

public interface IMoeTestStartup
{
    public void ConfigureService(IServiceCollection services, IConfiguration configuration);

    public int Order { get; }
}