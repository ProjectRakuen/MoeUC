using MoeUC.Core.Helpers;
using MoeUC.Test.CommonTests.Mocks;
using ProtoBuf;

namespace MoeUC.Test.CommonTests;

public class ConvertHelperTest : IClassFixture<ConvertHelper>
{
    [Fact]
    public void TestJsonConvert()
    {
        var mock = new MockProtoEntity()
        {
            Name = "Test",
            Num = 100
        };

        var json = ConvertHelper.JsonSerialize(mock);
        var obj = ConvertHelper.JsonDeserialize<MockProtoEntity>(json);
        
        Assert.NotNull(obj);
        Assert.Equal(mock.Num, obj.Num);
        Assert.Equal(mock.Num, obj.Num);
    }

    [Fact]
    public void TestProtoConvert()
    {
        var mock = new MockProtoEntity()
        {
            Name = "Proto",
            Num = 100
        };
        var proto = ConvertHelper.ProtoSerialize(mock);
        var obj = ConvertHelper.ProtoDeserialize<MockProtoEntity>(proto);

        Assert.NotNull(obj);
        Assert.Equal(mock.Name, obj.Name);
        Assert.Equal(mock.Num, obj.Num);
    }
}

