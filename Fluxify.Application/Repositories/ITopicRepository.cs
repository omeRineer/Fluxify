using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.Repositories
{
    public interface ITopicRepository : IEfCoreRepository<Topic, Guid>
    {
        Task<List<Topic>> GetListAsync(Guid channelId);
        Task<List<Topic>> GetListAsync(string[] topics);
    }
}
