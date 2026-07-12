using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Repositories.Core
{
    public class ChannelRepository : CoreRepository<Channel, Guid, CoreContext>, IChannelRepository
    {
        public ChannelRepository(CoreContext dbContext) : base(dbContext)
        {
        }
    }
}
