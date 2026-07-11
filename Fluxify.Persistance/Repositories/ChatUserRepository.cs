using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class ChatUserRepository : MongoRepository<ChatUser, Guid>, IChatUserRepository
    {
        public ChatUserRepository(IMongoDatabase dataBase) : base(dataBase, "ChatUsers")
        {
        }
    }
}
