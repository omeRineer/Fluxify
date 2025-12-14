using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Repositories
{
    public interface ISubscriberRepository : IMongoRepository<Subscriber, Guid>
    {
    }
}
