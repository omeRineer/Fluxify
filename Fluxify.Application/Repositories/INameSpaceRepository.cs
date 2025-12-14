using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Repositories
{
    public interface INameSpaceRepository : IMongoRepository<NameSpace, Guid>
    {
    }
}
