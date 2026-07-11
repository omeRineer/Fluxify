using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Repositories
{
    public class ChatRepository : MongoRepository<Chat, Guid>, IChatRepository
    {
        public ChatRepository(IMongoDatabase dataBase) : base(dataBase, "Chats")
        {
        }
    }
}
