using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Repositories.Mongo
{
    public interface IUserNotificationRepository : IMongoRepository<UserNotification, Guid>
    {
    }
}
