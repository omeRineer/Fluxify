using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class SubscriberRepository : MongoRepository<Subscriber, Guid>, ISubscriberRepository
    {
        public SubscriberRepository(IMongoDatabase dataBase) : base(dataBase, "Subscribers")
        {
        }
    }
}
