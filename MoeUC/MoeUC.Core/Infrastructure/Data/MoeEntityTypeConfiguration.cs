using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MoeUC.Core.Infrastructure.Data;

public class MoeEntityTypeConfiguration<TEntity, TId> : IMappingConfiguration, IEntityTypeConfiguration<TEntity> where TEntity : class
{
    /// <summary>
    /// Apply this configuration
    /// </summary>
    /// <param name="modelBuilder"></param>
    public void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(this);
    }

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {

    }
}