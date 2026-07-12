using Fluxify.Application.Repositories.Base;
using Fluxify.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class CoreRepository<TEntity, TKey, TContext> : IRepository<TEntity, TKey>
    where TEntity : CoreEntity<TKey>, new()
    where TContext : DbContext
{
    protected readonly TContext dbContext;

    public CoreRepository(TContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    protected DbSet<TEntity> Set => dbContext.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await Set.AddRangeAsync(entities);
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = await FindAsync(id);
        if (entity != null)
        {
            Set.Remove(entity);
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<TKey> ids)
    {
        var entities = await Set.Where(e => ids.Contains(e.Id)).ToListAsync();
        if (entities.Any())
        {
            Set.RemoveRange(entities);
        }
    }

    public async Task<TEntity> FindAsync(TKey id)
    {
        return await Set.FindAsync(id);
    }

    public async Task<Dictionary<TK, TV>> GetDictionariesAsync<TK, TV>(Func<TEntity, TK> key, Func<TEntity, TV> value, Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = Set;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var list = await query.ToListAsync();
        return list.ToDictionary(key, value);
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set.FirstAsync(filter);
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set.FirstOrDefaultAsync(filter);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null)
    {
        IQueryable<TEntity> query = Set;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            var result = await query.ToListAsync();
            return orderBy(result).ToList();
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set.SingleAsync(filter);
    }

    public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set.SingleOrDefaultAsync(filter);
    }

    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Set.AnyAsync(expression);
    }

    public Task UpdateAsync(TEntity entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        Set.UpdateRange(entities);
        return Task.CompletedTask;
    }
}