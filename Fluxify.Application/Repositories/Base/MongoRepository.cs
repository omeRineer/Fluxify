using Fluxify.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Repositories.Base
{
    public class MongoRepository<TEntity, TKey> : IMongoRepository<TEntity, TKey>
where TEntity : MongoEntity<TKey>, new()
    {
        protected readonly IMongoCollection<TEntity> Collection;


        public MongoRepository(IMongoDatabase dataBase, string collectionName)
        {
            Collection = dataBase.GetCollection<TEntity>(collectionName);
        }


        public async Task AddAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity, new InsertOneOptions
            {
                BypassDocumentValidation = true
            });
        }


        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var models = entities.Select(x => new InsertOneModel<TEntity>(x)).Cast<WriteModel<TEntity>>().ToList();


            await Collection.BulkWriteAsync(models, new BulkWriteOptions
            {
                IsOrdered = false
            });
        }


        public async Task DeleteAsync(TKey id)
        {
            await Collection.DeleteOneAsync(x => x.Id!.Equals(id));
        }


        public async Task DeleteRangeAsync(IEnumerable<TKey> ids)
        {
            await Collection.DeleteManyAsync(x => ids.Contains(x.Id));
        }


        public async Task<Dictionary<TK, TV>> GetDictionariesAsync<TK, TV>(Func<TEntity, TK> key, Func<TEntity, TV> value, Expression<Func<TEntity, bool>> filter = null)
        {
            var data = filter == null
            ? await Collection.Find(_ => true).ToListAsync()
            : await Collection.Find(filter).ToListAsync();


            return data.ToDictionary(key, value);
        }


        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        => await Collection.Find(filter).FirstAsync();


        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        => await Collection.Find(filter).FirstOrDefaultAsync();


        public async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderBy = null)
        {
            var list = filter == null
            ? await Collection.Find(_ => true).ToListAsync()
            : await Collection.Find(filter).ToListAsync();


            if (orderBy != null)
                list = orderBy(list).ToList();



            return list;
        }


        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
        => await Collection.Find(filter).SingleAsync();


        public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        => await Collection.Find(filter).SingleOrDefaultAsync();


        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Collection.Find(expression).AnyAsync();
        }


        public async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity);
        }


        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            var models = entities.Select(entity =>
            new ReplaceOneModel<TEntity>(
            Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id),
            entity
            )
            { IsUpsert = false }
            ).Cast<WriteModel<TEntity>>().ToList();


            if (models.Any())
                await Collection.BulkWriteAsync(models);
        }
    }
}
