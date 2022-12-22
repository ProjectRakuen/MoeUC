using Microsoft.EntityFrameworkCore;

namespace Moe.Core.Infrastructure.Data;

/// <summary>
/// DbContext model mapping configuration
/// </summary>
public interface IMappingConfiguration
{
    /// <summary>
    /// Apply this configuration
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the database context</param>
    void ApplyConfiguration(ModelBuilder modelBuilder);
}