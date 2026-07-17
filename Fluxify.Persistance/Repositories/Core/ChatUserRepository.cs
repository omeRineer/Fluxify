using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class ChatUserRepository : CoreRepository<ChatUser, Guid>, IChatUserRepository
    {
        public ChatUserRepository(CoreContext context) : base(context)
        {
        }
    }
}
