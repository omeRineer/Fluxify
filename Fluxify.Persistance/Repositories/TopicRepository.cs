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
    }
}
