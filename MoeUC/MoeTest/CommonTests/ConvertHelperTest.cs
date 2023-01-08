using MoeUC.Core.Helpers;
using ProtoBuf;

namespace MoeUC.Test.CommonTests;

public class ConvertHelperTest : IClassFixture<ConvertHelper>
{
    [Fact]
    public void TestJsonConvert()
    {
        var mock = new MockEntity()
        {
            Name = "Test",
            Num = 100
        };

        var json = ConvertHelper.JsonSerialize(mock);
        var obj = ConvertHelper.JsonDeserialize<MockEntity>(json);

        Assert.Equal(mock.Num, obj.Num);
        Assert.Equal(mock.Num, obj.Num);
    }

    [Fact]
    public void TestProtoConvert()
    {
        var mock = new MockEntity()
        {
            Name = "Proto",
            Num = 100
        };
        var proto = ConvertHelper.ProtoSerialize(mock);
        var obj = ConvertHelper.ProtoDeserialize<MockEntity>(proto);

        Assert.Equal(mock.Name, obj.Name);
        Assert.Equal(mock.Num, obj.Num);
    }
}

[ProtoContract]
public class MockEntity
{
    [ProtoMember(1)]
    public string? Name { get; set; }

    [ProtoMember(2)]
    public int Num { get; set; }
}