using System.Linq.Expressions;
using MoeUC.Core.Infrastructure.Data;
using MoeUC.Core.Infrastructure.Dependency;
using MoeUC.Service.ServiceBase.Models;

namespace MoeUC.Service.ServiceBase;

/// <summary>
/// BaseService for a entity
/// </summary>
/// <typeparam name="TEntity">Concrete type of the entity</typeparam>
/// <typeparam name="TId">Identity type of the entity</typeparam>
public class BaseService<TEntity, TId> : IScoped where TEntity : BaseEntity<TId>
{
    private readonly IRepository<TEntity, TId> _repository;
    protected readonly WorkContext WorkContext;

    protected BaseService(IRepository<TEntity, TId> repository, WorkContext workContext)
    {
        _repository = repository;
        WorkContext = workContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        WorkContext.RequestStatistic.DbRead++;
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        WorkContext.RequestStatistic.DbWrite += await _repository.InsertAsync(entity);
    }

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        WorkContext.RequestStatistic.DbWrite  += await _repository.InsertAsync(entities);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        WorkContext.RequestStatistic.DbWrite += await _repository.UpdateAsync(entity);
    }

    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        WorkContext.RequestStatistic.DbWrite += await _repository.UpdateAsync(entities);
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        WorkContext.RequestStatistic.DbWrite += await _repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        WorkContext.RequestStatistic.DbWrite += await _repository.DeleteAsync(entities);
    }

    public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where, bool tracking = false)
    {
        return (tracking ? _repository.Table : _repository.TableNoTracking)
            .Where(where);
    }

    public virtual IQueryable<TEntity> FindAll(bool tracking = false)
    {
        return tracking ? _repository.Table : _repository.TableNoTracking;
    }
}

/// <summary>
/// BaseService for a entity with a default identity
/// type of int
/// </summary>
/// <typeparam name="T">Concrete type of the entity</typeparam>
public class BaseService<T> : BaseService<T, int> where T : BaseEntity<int>
{
    public BaseService(IRepository<T> repository, WorkContext workContext) : base(repository, workContext)
    {
    }

}