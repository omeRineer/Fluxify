using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Repositories
{
    public interface IChannelRepository : IMongoRepository<Channel, Guid>
    {
    }
}
