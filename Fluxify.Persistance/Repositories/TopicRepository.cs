using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class TopicRepository : MongoRepository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(IMongoDatabase dataBase) : base(dataBase, "Topics")
        {
        }

        /// <summary>
        /// By Channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetListAsync(Guid channelId)
            => await GetListAsync(f => f.ChannelId == channelId);

        /// <summary>
        /// By Topics
        /// </summary>
        /// <param name="topics"></param>
        /// <returns></returns>
        public async Task<List<Topic>> GetListAsync(params string[] keys)
            => await GetListAsync(f => keys.Contains(f.Key));
    }
}
