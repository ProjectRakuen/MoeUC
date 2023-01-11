using ProtoBuf;

namespace MoeUC.Test.CommonTests.Mocks;

[ProtoContract]
public class MockProtoEntity
{
    [ProtoMember(1)]
    public string? Name { get; set; }

    [ProtoMember(2)]
    public int Num { get; set; }

    [ProtoMember(3)]
    public int Id { get; set; }

    public bool Equals(MockProtoEntity? entity)
    {
        return entity != null && entity.Num == Num && entity.Id == Id && entity.Name == Name;
    }
}