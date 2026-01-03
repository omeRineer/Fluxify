using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Repositories
{
    public interface IUserTopicRepository : IMongoRepository<UserTopic, Guid>
    {
    }
}
