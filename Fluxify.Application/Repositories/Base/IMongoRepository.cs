using Fluxify.Application.Models.Common.Pagination;
using Fluxify.Domain;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Fluxify.Application.Repositories.Base
{
    public interface IMongoRepository<TDocument, TKey>
        where TDocument : MongoEntity<TKey>
    {
        #region Query

        IQueryable<TDocument> AsQueryable();

        IAggregateFluent<TDocument> Aggregate();

        Task<long> CountAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<TDocument?> GetByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default);

        Task<TDocument?> FirstOrDefaultAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<List<TDocument>> FindAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<List<TDocument>> GetListAsync(
            Expression<Func<TDocument, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<PagedData<TDocument>> GetPagedAsync(
            int page = 0,
            int size = 10,
            Expression<Func<TDocument, bool>>? filter = null,
            SortDefinition<TDocument>? sort = null,
            CancellationToken cancellationToken = default);

        #endregion


        #region Insert

        Task InsertAsync(
            TDocument document,
            CancellationToken cancellationToken = default);

        Task InsertManyAsync(
            IEnumerable<TDocument> documents,
            CancellationToken cancellationToken = default);

        #endregion


        #region Replace

        Task ReplaceAsync(
            TDocument document,
            CancellationToken cancellationToken = default);

        #endregion


        #region Update

        Task<UpdateResult> UpdateOneAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);


        Task<UpdateResult> UpdateManyAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);


        Task<TDocument?> FindOneAndUpdateAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool returnUpdated = true,
            CancellationToken cancellationToken = default);


        Task<TDocument?> FindOneAndReplaceAsync(
            Expression<Func<TDocument, bool>> filter,
            TDocument document,
            bool returnUpdated = true,
            CancellationToken cancellationToken = default);

        #endregion


        #region Delete

        Task DeleteByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default);


        Task DeleteOneAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);


        Task DeleteManyAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);


        Task<TDocument?> FindOneAndDeleteAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default);

        #endregion


        #region Bulk

        Task<BulkWriteResult<TDocument>> BulkWriteAsync(
            IEnumerable<WriteModel<TDocument>> operations,
            CancellationToken cancellationToken = default);

        #endregion
    }
}