using MoeUC.Core.Infrastructure.Mapping;
using MoeUC.Test.CommonTests.Mocks;

namespace MoeUC.Test.CommonTests;

public class MapperTest : IClassFixture<MapperHelper>
{
    [Fact]
    public void Test()
    {
        var source = new MockMapSource()
        {
            Id = 1,
            Name = "Test",
            Num = 100,
        };

        var dest = source.MapTo<MockMapDestination>();
        Assert.Equal(source.Id, dest.Id);
        Assert.Equal(source.Name, dest.Name);
        Assert.Equal(source.Num, dest.Num);
    }
}