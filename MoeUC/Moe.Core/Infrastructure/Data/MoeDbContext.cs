using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Moe.Core.Infrastructure.Data;

public class MoeDbContext : DbContext
{
    public MoeDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// configure the model
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Load all entity and query type configurations dynamically.
        var typeConfigs = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(c =>
                (c.BaseType?.IsGenericType ?? false) &&
                c.BaseType.GetGenericTypeDefinition() == typeof(MoeEntityTypeConfiguration<,>) &&
                c.BaseType.GenericTypeArguments.Any(d => d.AllBaseTypes()
                    .Any(e => e.IsGenericType && e.GetGenericTypeDefinition().IsAssignableFrom(typeof(BaseEntity<>)))))
            .ToList();

        foreach (var typeConfig in typeConfigs)
        {

            var config = Activator.CreateInstance(typeConfig);
            if (config != null)
            {
                ((IMappingConfiguration)config).ApplyConfiguration(modelBuilder);
            }
        }

        // Apply config to all classes that extend BaseEntity
        var entityTypes = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(c =>
            {
                try
                {
                    return c.GetTypes();
                }
                catch
                {
                    return Type.EmptyTypes;
                }
            })
            .Where(c =>
                c != typeof(BaseEntity<>) &&
                c != typeof(BaseEntity) &&
                c.GetCustomAttribute<NotMappedAttribute>() == null &&
                c.AllBaseTypes()
                    .Any(d => d.IsGenericType && d.GetGenericTypeDefinition().IsAssignableFrom(typeof(BaseEntity<>))))
            .ToList();

        foreach (var entityType in entityTypes)
        {
            var keyType = entityType
                .AllBaseTypes()
                .First(c => c.IsGenericType && c.GetGenericTypeDefinition().IsAssignableFrom(typeof(BaseEntity<>)))
                .GetGenericArguments()
                .First();
            dynamic? entityConfig = Activator
                .CreateInstance(typeof(MoeEntityTypeConfiguration<,>).MakeGenericType(entityType, keyType));
            if (entityConfig != null)
            {
                modelBuilder.ApplyConfiguration(entityConfig);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Generate a script to create all tables for the current model
    /// </summary>
    /// <returns>A SQL script</returns>
    public virtual string GenerateCreateScript()
    {
        return Database.GenerateCreateScript();
    }
}