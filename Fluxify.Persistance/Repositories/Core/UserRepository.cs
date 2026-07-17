using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class UserRepository : CoreRepository<User, Guid>, IUserRepository
    {
        public UserRepository(CoreContext dbContext) : base(dbContext)
        {
        }
    }
}
