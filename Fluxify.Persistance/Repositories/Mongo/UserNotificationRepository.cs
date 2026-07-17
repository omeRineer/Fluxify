using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Domain.Enums;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Mongo
{
    public class UserNotificationRepository : MongoRepository<UserNotification, Guid>, IUserNotificationRepository
    {
        protected override string GetCollectionName()
            => "UserNotifications";

        public UserNotificationRepository(MongoContext context) : base(context)
        {
        }
                
    }
}
