using System.NetPro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Infrastructure.StartupConfigs;

namespace MoeUC.Test;

public class Startup
{
    private List<IMoeStartup> configInstances = null!;

    // Startup config for all tests
    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        // Add settings
        context.Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        var config = context.Configuration;

        var typeFinder = new WebAppTypeFinder(new TypeFinderOption()
            {
                MountePath = ".\\"
            }, config,
            new NetProFileProvider(context.HostingEnvironment));

        var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>(true);
        var registrarInstances = dependencyRegistrars
            .Select(c => (IDependencyRegistrar)Activator.CreateInstance(c)!)
            .OrderBy(c => c.Order)
            .ToList();

        foreach (var registrar in registrarInstances)
        {
            registrar.Register(services, typeFinder, config);
        }

        // Run service startup configs
        var startupConfigs = typeFinder.FindClassesOfType<IMoeStartup>(true).ToList();
        configInstances = startupConfigs
            .Select(c => (IMoeStartup)Activator.CreateInstance(c)!)
            .OrderBy(c => c.Order)
            .ToList();

        // configure services
        foreach (var item in configInstances)
        {
            item.ConfigureServices(services, config);
        }

        var serviceProvider = services.BuildServiceProvider();
        
        ApplicationContext.Init(serviceProvider);
    }

}