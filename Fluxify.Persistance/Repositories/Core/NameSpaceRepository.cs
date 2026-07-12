using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class NameSpaceRepository : CoreRepository<NameSpace, Guid, CoreContext>, INameSpaceRepository
    {
        public NameSpaceRepository(CoreContext dbContext) : base(dbContext)
        {
        }
    }
}
