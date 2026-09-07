using Fluxify.Application.Models.Common.Pagination;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain;
using Fluxify.Persistance.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Repositories.Base
{
    public abstract class MongoRepository<TDocument, TKey> : IMongoRepository<TDocument, TKey>
        where TDocument : MongoEntity<TKey>
    {
        protected IMongoCollection<TDocument> Collection { get; }

        protected MongoRepository(MongoContext context)
        {
            Collection = context.Database.GetCollection<TDocument>(
                GetCollectionName());
        }

        protected abstract string GetCollectionName();

        #region Query

        public virtual IQueryable<TDocument> AsQueryable()
            => Collection.AsQueryable();

        public virtual IAggregateFluent<TDocument> Aggregate()
            => Collection.Aggregate();

        public virtual Task<long> CountAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await Collection.Find(filter)
                .Limit(1)
                .AnyAsync(cancellationToken);
        }

        public virtual Task<bool> ExistsAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
            => AnyAsync(filter, cancellationToken);

        public virtual Task<TDocument?> GetByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default)
        {
            return Collection.Find(x => x.Id!.Equals(id))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<TDocument?> FirstOrDefaultAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.Find(filter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<List<TDocument>> FindAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.Find(filter)
                .ToListAsync(cancellationToken);
        }

        public virtual Task<List<TDocument>> GetListAsync(
            Expression<Func<TDocument, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            filter ??= _ => true;

            return Collection
                .Find(filter)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<PageData<TDocument>> GetPagedAsync(
            int page = 0,
            int size = 10,
            Expression<Func<TDocument, bool>>? filter = null,
            SortDefinition<TDocument>? sort = null,
            CancellationToken cancellationToken = default)
        {
            filter ??= _ => true;

            sort ??= Builders<TDocument>.Sort.Ascending(x => x.Id);

            var totalCountTask = Collection.CountDocumentsAsync(
                filter,
                cancellationToken: cancellationToken);

            var itemsTask = Collection
                .Find(filter)
                .Sort(sort)
                .Skip(page * size)
                .Limit(size)
                .ToListAsync(cancellationToken);

            await Task.WhenAll(totalCountTask, itemsTask);

            return new PageData<TDocument>
            {
                Data = await itemsTask,
                TotalCount = await totalCountTask,
                Page = page,
                Size = size
            };
        }

        #endregion

        #region Insert

        public virtual Task InsertAsync(
            TDocument document,
            CancellationToken cancellationToken = default)
        {
            return Collection.InsertOneAsync(
                document,
                cancellationToken: cancellationToken);
        }

        public virtual Task InsertManyAsync(
            IEnumerable<TDocument> documents,
            CancellationToken cancellationToken = default)
        {
            return Collection.InsertManyAsync(
                documents,
                cancellationToken: cancellationToken);
        }

        #endregion

        #region Replace

        public virtual Task ReplaceAsync(
            TDocument document,
            CancellationToken cancellationToken = default)
        {
            return Collection.ReplaceOneAsync(
                x => x.Id!.Equals(document.Id),
                document,
                cancellationToken: cancellationToken);
        }

        #endregion

        #region Update

        public virtual Task<UpdateResult> UpdateOneAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return Collection.UpdateOneAsync(
                filter,
                update,
                new UpdateOptions { IsUpsert = isUpsert },
                cancellationToken);
        }

        public virtual Task<UpdateResult> UpdateManyAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return Collection.UpdateManyAsync(
                filter,
                update,
                new UpdateOptions { IsUpsert = isUpsert },
                cancellationToken);
        }

        public virtual Task<TDocument?> FindOneAndUpdateAsync(
            Expression<Func<TDocument, bool>> filter,
            UpdateDefinition<TDocument> update,
            bool returnUpdated = true,
            CancellationToken cancellationToken = default)
        {
            return Collection.FindOneAndUpdateAsync(
                filter,
                update,
                new FindOneAndUpdateOptions<TDocument>
                {
                    ReturnDocument = returnUpdated
                        ? ReturnDocument.After
                        : ReturnDocument.Before
                },
                cancellationToken);
        }

        public virtual Task<TDocument?> FindOneAndReplaceAsync(
            Expression<Func<TDocument, bool>> filter,
            TDocument document,
            bool returnUpdated = true,
            CancellationToken cancellationToken = default)
        {
            return Collection.FindOneAndReplaceAsync(
                filter,
                document,
                new FindOneAndReplaceOptions<TDocument>
                {
                    ReturnDocument = returnUpdated
                        ? ReturnDocument.After
                        : ReturnDocument.Before
                },
                cancellationToken);
        }

        #endregion

        #region Delete

        public virtual Task DeleteByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOneAsync(
                x => x.Id!.Equals(id),
                cancellationToken);
        }

        public virtual Task DeleteOneAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOneAsync(
                filter,
                cancellationToken);
        }

        public virtual Task DeleteManyAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.DeleteManyAsync(
                filter,
                cancellationToken);
        }

        public virtual Task<TDocument?> FindOneAndDeleteAsync(
            Expression<Func<TDocument, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return Collection.FindOneAndDeleteAsync(
                filter,
                cancellationToken: cancellationToken);
        }

        #endregion

        #region Bulk

        public virtual Task<BulkWriteResult<TDocument>> BulkWriteAsync(
            IEnumerable<WriteModel<TDocument>> operations,
            CancellationToken cancellationToken = default)
        {
            return Collection.BulkWriteAsync(
                operations,
                cancellationToken: cancellationToken);
        }

        #endregion
    }
}
