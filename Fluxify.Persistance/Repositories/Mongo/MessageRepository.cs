using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Mongo
{
    public class MessageRepository : MongoRepository<Message, Guid>, IMessageRepository
    {
        protected override string GetCollectionName()
            => "Messages";
        public MessageRepository(MongoContext context) : base(context)
        {
        }
    }
}
