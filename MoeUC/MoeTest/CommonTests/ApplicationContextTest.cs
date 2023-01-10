using Microsoft.Extensions.Logging;
using MoeUC.Core.Infrastructure.Dependency;

namespace MoeUC.Test.CommonTests;

public class ApplicationContextTest
{
    [Fact]
    public void Test()
    {
        var logger = ApplicationContext.Resolve<ILogger<ApplicationContextTest>>();
        Assert.NotNull(logger);
    }
}