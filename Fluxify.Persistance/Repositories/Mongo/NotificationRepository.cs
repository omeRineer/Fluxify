using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Mongo;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Mongo
{
    public class NotificationRepository : MongoRepository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(IMongoDatabase dataBase) : base(dataBase, "Notifications")
        {
        }
    }
}
