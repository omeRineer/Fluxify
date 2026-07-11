using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class MessageRepository : MongoRepository<Message, Guid>, IMessageRepository
    {
        public MessageRepository(IMongoDatabase dataBase) : base(dataBase, "Messages")
        {
        }
    }
}
