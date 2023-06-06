using Microsoft.EntityFrameworkCore;

namespace MoeUC.Core.Infrastructure.Data;


/// <summary>
/// Represents the Entity Framework Core repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public class EntityFrameworkCoreRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    private readonly MoeDbContext _dbContext;

    private DbSet<TEntity>? _entityDbSet;

    public EntityFrameworkCoreRepository(MoeDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    protected virtual DbSet<TEntity> Entities
    {
        get { return _entityDbSet ??= _dbContext.Set<TEntity>(); }
    }

    public virtual IQueryable<TEntity> Table => Entities;

    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    public virtual async Task<TEntity> GetByIdAsync(TId id)
    {
        var entity = await Entities.FindAsync(id);
        if (entity == null)
        {
            throw new NullReferenceException($"No entity found with id {id}");
        }
        return entity;
    }

    public async Task<int> InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            await Entities.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }

        return 0;
    }

    public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            await Entities.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }
    }

    public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.UpdateRange(entities);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }
    }

    public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            Entities.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            //ensure that the detailed error text is saved in the Log
            throw new Exception(GetErrorMessageAndRollBackChanges(exception), exception);
        }
    }

    /// <summary>
    /// RollBack entity changes and return error message
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected string GetErrorMessageAndRollBackChanges(DbUpdateException exception)
    {
        // rollback changes
        if (this._dbContext is DbContext dbContext)
        {
            var entries = dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            entries.ForEach(entry =>
            {
                try
                {
                    entry.State = EntityState.Unchanged;
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            });
        }

        try
        {
            this._dbContext.SaveChanges();
            return exception.ToString();
        }
        catch (Exception ex)
        {
            //if after the rollback of changes the context is still not saving,
            //return the full text of the exception that occurred when saving
            return ex.ToString();
        }
    }
}

/// <summary>
/// EntityFrameworkRepository used by a default identity type of int
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityFrameworkCoreRepository<TEntity> : EntityFrameworkCoreRepository<TEntity, int>, IRepository<TEntity>
    where TEntity : BaseEntity<int>
{
    public EntityFrameworkCoreRepository(MoeDbContext dbContext) : base(dbContext)
    {
    }
}
