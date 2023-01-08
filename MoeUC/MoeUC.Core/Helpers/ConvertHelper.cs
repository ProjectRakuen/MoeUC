using System.Text.Json;
using System.Text.Json.Serialization;
using ProtoBuf;

namespace MoeUC.Core.Helpers;

public class ConvertHelper
{
    public static T JsonDeserialize<T>(string json)
    {
        var obj = JsonSerializer.Deserialize<T>(json);
        if (obj == null)
            throw new JsonException("deserialized null value");
        return obj;
    }

    public static string JsonSerialize(object obj)
    {
        
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });

        if (string.IsNullOrEmpty(json))
            throw new JsonException("Json Serialize failed");

        return json;
    }

    public static byte[] ProtoSerialize(object obj)
    {
        using var memoryStream = new MemoryStream();
        Serializer.Serialize(memoryStream, obj);

        return memoryStream.ToArray();
    }

    public static T ProtoDeserialize<T>(byte[] data)
    {
        using var stream = new MemoryStream(data);
        return Serializer.Deserialize<T>(stream);
    }
}

public enum MoeSerializeType
{
    Json = 0,
    Proto = 1
}