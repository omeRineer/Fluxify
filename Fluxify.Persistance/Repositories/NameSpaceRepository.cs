using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class NameSpaceRepository : MongoRepository<NameSpace, Guid>, INameSpaceRepository
    {
        public NameSpaceRepository(IMongoDatabase dataBase) : base(dataBase, "NameSpaces")
        {
        }
    }
}
