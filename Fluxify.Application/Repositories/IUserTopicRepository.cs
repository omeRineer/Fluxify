using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.Repositories
{
    public interface IUserTopicRepository : IEfCoreRepository<UserTopic, Guid>
    {
    }
}
