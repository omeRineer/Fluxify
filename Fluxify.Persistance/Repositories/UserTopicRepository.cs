using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class UserTopicRepository : MongoRepository<UserTopic, Guid>, IUserTopicRepository
    {
        public UserTopicRepository(IMongoDatabase dataBase) : base(dataBase, "UserTopics")
        {
        }
    }
}
