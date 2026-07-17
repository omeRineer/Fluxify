using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Repositories
{
    public interface IChannelRepository : IEfCoreRepository<Channel, Guid>
    {
    }
}
