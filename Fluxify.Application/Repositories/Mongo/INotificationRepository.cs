using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Mongo;

namespace Fluxify.Application.Repositories.Mongo
{
    public interface INotificationRepository : IMongoRepository<Notification, Guid>
    {
    }
}
