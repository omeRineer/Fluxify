using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class UserRepository : CoreRepository<User, Guid, CoreContext>, IUserRepository
    {
        public UserRepository(CoreContext dbContext) : base(dbContext)
        {
        }
    }
}
