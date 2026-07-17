using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Mongo
{
    public class NotificationRepository : MongoRepository<Notification, Guid>, INotificationRepository
    {
        protected override string GetCollectionName()
            => "Notifications";
        public NotificationRepository(MongoContext context) : base(context)
        {
        }


    }
}
