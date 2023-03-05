using Microsoft.Extensions.DependencyInjection;

namespace MoeUC.Core.Infrastructure.Dependency;

public static class ApplicationContext
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public static void Init(IServiceProvider services)
    {
        ServiceProvider = services;
    }

    public static T Resolve<T>()
    {
        var result = ServiceProvider.GetService<T>();

        if (result == null)
            throw new ArgumentException("Type Not Found");

        return result;
    }

    public static List<T> ResolveAll<T>()
    {
        var result = ServiceProvider.GetServices<T>();
        return result.ToList();
    }
}