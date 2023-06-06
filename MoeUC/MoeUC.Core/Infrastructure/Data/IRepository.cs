namespace MoeUC.Core.Infrastructure.Data;

public interface IRepository<TEntity, in TId> where TEntity : BaseEntity<TId>
{
    /// <summary>
    /// Get a table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    /// <summary>
    /// Get a table with no tracking enabled 
    /// </summary>
    IQueryable<TEntity> TableNoTracking { get; }

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync(TId id);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<int> InsertAsync(TEntity entity);

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<int> InsertAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity"></param>
    Task<int> UpdateAsync(TEntity entity);

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities"></param>
    Task<int> UpdateAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity"></param>
    Task<int> DeleteAsync(TEntity entity);

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities"></param>
    Task<int> DeleteAsync(IEnumerable<TEntity> entities);
}

/// <summary>
/// IRepository used for a default identity
/// type of int
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : BaseEntity<int>
{

}