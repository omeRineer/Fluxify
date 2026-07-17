using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class TopicRepository : CoreRepository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(CoreContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Topic>> GetListAsync(Guid channelId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetListAsync(string[] topics)
        {
            throw new NotImplementedException();
        }
    }
}
