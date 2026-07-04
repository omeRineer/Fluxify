using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class UserRepository : MongoRepository<User, Guid>, IUserRepository
    {
        public UserRepository(IMongoDatabase dataBase) : base(dataBase, "Users")
        {
        }

    }
}
