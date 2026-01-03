using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using Fluxify.Domain.Enums;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class UserNotificationRepository : MongoRepository<UserNotification, Guid>, IUserNotificationRepository
    {
        public UserNotificationRepository(IMongoDatabase dataBase) : base(dataBase, "UserNotifications")
        {
        }


        public async Task MarkAsStatusAsync(NotificationStatus status, params Guid[] ids) =>
            await Collection.UpdateManyAsync(
                Builders<UserNotification>.Filter.And
                (
                    Builders<UserNotification>.Filter.In(x => x.Id, ids)
                ),
                Builders<UserNotification>.Update.Set(x => x.Status, (int)status));

        public async Task MarkAsStatusByNotificationAsync(NotificationStatus status, Guid? userId = null, params Guid[] notificationIds)
        {
            var filter = Builders<UserNotification>.Filter.In(x => x.NotificationId, notificationIds) &
        (userId.HasValue ? Builders<UserNotification>.Filter.Eq(x => x.UserId, userId.Value)
                         : Builders<UserNotification>.Filter.Empty);

            await Collection.UpdateManyAsync(
                filter,
                Builders<UserNotification>.Update.Set(x => x.Status, (int)status)
            );
        }
    }
}
