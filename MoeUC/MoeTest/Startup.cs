using System.NetPro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Core.Infrastructure.StartupConfigs;
using MoeUC.Test.StartUps;

namespace MoeUC.Test;

public class Startup
{
    private List<IMoeTestStartup> _configInstances = null!;

    // Startup config for all tests
    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        // Add settings
        context.Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        var currentDir = Directory.GetCurrentDirectory();

        var config = context.Configuration;
        services.AddSingleton(config);

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
        var startupConfigs = typeFinder.FindClassesOfType<IMoeTestStartup>(true).ToList();
        _configInstances = startupConfigs
            .Select(c => (IMoeTestStartup)Activator.CreateInstance(c)!)
            .OrderBy(c => c.Order)
            .ToList();

        // configure services
        foreach (var item in _configInstances)
        {
            item.ConfigureService(services, config);
        }

        //var serviceProvider = services.BuildServiceProvider();
        
        //ApplicationContext.Init(serviceProvider);
    }

}