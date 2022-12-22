using System.Linq.Expressions;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Core.Infrastructure.Dependency;

namespace MoeUC.Core.Infrastructure.ServiceBase;

/// <summary>
/// BaseService for a entity
/// </summary>
/// <typeparam name="TEntity">Concrete type of the entity</typeparam>
/// <typeparam name="TId">Identity type of the entity</typeparam>
public class BaseService<TEntity, TId> : IScoped where TEntity : BaseEntity<TId>
{
    protected readonly IRepository<TEntity, TId> Repository;

    public BaseService(IRepository<TEntity, TId> repository)
    {
        Repository = repository;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await Repository.GetByIdAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await Repository.InsertAsync(entity);
    }

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await Repository.InsertAsync(entities);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        await Repository.UpdateAsync(entity);
    }

    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        await Repository.UpdateAsync(entities);
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        await Repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        await Repository.DeleteAsync(entities);
    }

    public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where, bool tracking = false)
    {
        return (tracking ? Repository.Table : Repository.TableNoTracking)
            .Where(where);
    }

    public virtual IQueryable<TEntity> FindAll(bool tracking = false)
    {
        return tracking ? Repository.Table : Repository.TableNoTracking;
    }
}

/// <summary>
/// BaseService for a entity with a default identity
/// type of int
/// </summary>
/// <typeparam name="T">Concrete type of the entity</typeparam>
public class BaseService<T> : BaseService<T, int> where T : BaseEntity<int>
{
    public BaseService(IRepository<T> repository) : base(repository)
    {
    }

}