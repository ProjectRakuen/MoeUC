using System.ComponentModel.DataAnnotations;

namespace Moe.Core.Infrastructure.Data;

/// <summary>
/// Base class for an entity
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseEntity<T>
{
    [Key]
    public virtual T Id { get; set; } = default!;
}

/// <summary>
/// Non-generic class for an entity with a default identity
/// type of int
/// </summary>
public abstract class BaseEntity : BaseEntity<int>
{
}