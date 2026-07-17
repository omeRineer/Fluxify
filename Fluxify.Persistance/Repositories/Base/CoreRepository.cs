using Fluxify.Application.Repositories.Base;
using Fluxify.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Repositories.Base
{
    public class CoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity, TKey>
        where TEntity : CoreEntity<TKey>, new()
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> Table;
        public CoreRepository(DbContext context)
        {
            _context = context;
            Table = _context.Set<TEntity>();
        }


        #region Async
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null,
                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                      bool isTracking = true)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (!isTracking) query = query.AsNoTracking();
            if (filter != null) query = query.Where(filter);
            if (includes != null) query = includes(query);
            if (orderBy != null) query = orderBy(query);

            return await query.ToListAsync();
        }


        public async Task<Dictionary<TK, TV>> GetDictionariesAsync<TK, TV>(Func<TEntity, TK> key, Func<TEntity, TV> value, Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (filter != null) query = query.Where(filter);

            return await query.ToDictionaryAsync(key, value);
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                            bool isTracking = true)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (!isTracking) query = query.AsNoTracking();
            if (includes != null) query = includes(query);

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                            bool isTracking = true)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (!isTracking) query = query.AsNoTracking();
            if (includes != null) query = includes(query);

            return await query.SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                            bool isTracking = true)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (!isTracking) query = query.AsNoTracking();
            if (includes != null) query = includes(query);

            return await query.FirstAsync(filter);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                            bool isTracking = true)
        {
            IQueryable<TEntity> query = Table.AsQueryable();

            if (!isTracking) query = query.AsNoTracking();
            if (includes != null) query = includes(query);

            return await query.SingleAsync(filter);
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Table.AnyAsync(expression);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task ExecuteSqlAsync(string query, params object[] parameters)
        {
            await _context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Remove(entity);

            await Task.CompletedTask;
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);

            await Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);

            await Task.CompletedTask;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            => await GetListAsync(filter, null, orderBy, false);

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
            => await GetFirstOrDefaultAsync(filter, null, false);

        public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
            => await GetSingleOrDefaultAsync(filter, null, false);

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
            => await GetFirstAsync(filter, null, false);

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
            => await GetSingleAsync(filter, null, false);

        public async Task DeleteAsync(TKey id)
        {
            var entity = await Table.FindAsync(id);
            Table.Remove(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<TKey> ids)
        {
            var entities = Table.Where(f => ids.Contains(f.Id));
            Table.RemoveRange(entities);

            await Task.CompletedTask;
        }

        public async Task<TEntity> FindAsync(TKey id)
        {
            return await Table.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TResult>> GetProjectedListAsync<TResult>(
                Expression<Func<TEntity, TResult>> select,
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(select).ToListAsync();
        }

        public async Task<TResult?> GetProjectedFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<TResult?> GetProjectedSingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.Select(select).SingleOrDefaultAsync();
        }


        #endregion
    }
}
