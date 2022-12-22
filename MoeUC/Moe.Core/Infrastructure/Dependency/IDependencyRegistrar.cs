using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.NetPro;

namespace Moe.Core.Infrastructure.Dependency;

public interface IDependencyRegistrar
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="typeFinder"></param>
    /// <param name="configuration"></param>
    void Register(IServiceCollection service, ITypeFinder typeFinder, IConfiguration configuration);

    /// <summary>
    /// Get registering order
    /// </summary>
    int Order { get; }
}