using System.Reflection;
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

    public static byte[] AutoSerialize(object obj)
    {
        if (CanProtoSerialize(obj.GetType()))
            return ProtoSerialize(obj);

        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }

    public static T AutoDeserialize<T>(byte[] objectBytes)
    {
        if (CanProtoSerialize(typeof(T)))
            return ProtoDeserialize<T>(objectBytes);

        var obj = JsonSerializer.Deserialize<T>(objectBytes);
        if (obj == null)
            throw new JsonException("json deserialized to null");

        return obj;
    }

    public static bool CanProtoSerialize(Type? type)
    {
        if (type == null)
            return false;
        
        // get generic type of IEnumerable
        if (type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && type.GenericTypeArguments.Length == 1)
        {
            type = type.GenericTypeArguments.First();
        }

        return type.GetCustomAttribute(typeof(ProtoContractAttribute), false) != null;
    }
}

public enum MoeSerializeType
{
    Json = 0,
    Proto = 1
}