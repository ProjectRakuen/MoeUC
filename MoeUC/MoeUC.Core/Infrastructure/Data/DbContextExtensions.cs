namespace MoeUC.Core.Infrastructure.Data;

public static class DbContextExtensions
{
    /// <summary>
    /// Get all base types of the given type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> AllBaseTypes(this Type? type)
    {
        for (var current = type; current != null; current = current.BaseType)
            yield return current;
    }
}