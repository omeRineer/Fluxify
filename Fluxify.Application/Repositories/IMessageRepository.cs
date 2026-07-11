using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Repositories
{
    public interface IMessageRepository : IMongoRepository<Message, Guid>
    {
    }
}
