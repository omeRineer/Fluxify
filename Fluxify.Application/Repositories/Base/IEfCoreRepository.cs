using Fluxify.Domain;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Repositories.Base
{
    public interface IEfCoreRepository<TEntity, TKey>
    {
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null,
                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                   bool isTracking = true);

        //Task<PageData<TEntity>> GetListByPaginationAsync(PaginationParameter paginationParameter,
        //                                 Expression<Func<TEntity, bool>> filter = null,
        //                                 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
        //                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //                                 bool isTracking = true);

        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                         bool isTracking = true);

        Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter,
                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                         bool isTracking = true);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter,
                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                         bool isTracking = true);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter,
                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
                         bool isTracking = true);

        Task<List<TResult>> GetProjectedListAsync<TResult>(
                        Expression<Func<TEntity, TResult>> select,
                        Expression<Func<TEntity, bool>> filter = null,
                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TResult?> GetProjectedFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> filter);

        Task<TResult?> GetProjectedSingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>> filter);

        Task ExecuteSqlAsync(string query, params object[] parameters);

        Task<TEntity> FindAsync(TKey id);
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TKey id);
        Task DeleteRangeAsync(IEnumerable<TKey> ids);

        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task SaveChangesAsync();
    }
}
