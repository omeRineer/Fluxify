using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Repositories
{
    public interface ITopicRepository : IMongoRepository<Topic, string>
    {
        Task<List<Topic>> GetListAsync(Guid channelId);
        Task<List<Topic>> GetListAsync(string[] topics);
    }
}
