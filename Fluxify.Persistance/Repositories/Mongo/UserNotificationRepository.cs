using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Domain.Enums;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Mongo
{
    public class UserNotificationRepository : MongoRepository<UserNotification, Guid>, IUserNotificationRepository
    {
        public UserNotificationRepository(IMongoDatabase dataBase) : base(dataBase, "UserNotifications")
        {
        }

        public Task<List<UserNotification>> GetListAsync(Guid notificationId)
            => GetListAsync(f => f.NotificationId == notificationId);

        public async Task MarkAsStatusAsync(NotificationStatus status, Guid[] notifications, Guid[]? users = null)
        {
            var filters = new List<FilterDefinition<UserNotification>>
            {
                Builders<UserNotification>
                    .Filter
                    .In(x => x.NotificationId, notifications)
            };

            if (users is { Length: > 0 })
            {
                filters.Add(
                    Builders<UserNotification>
                        .Filter
                        .In(x => x.UserId, users));
            }

            var filter = Builders<UserNotification>
                .Filter
                .And(filters);

            //var update = Builders<UserNotification>
            //    .Update
            //    .Set(x => x.Status, (int)status);

            //await Collection.UpdateManyAsync(filter, update);
        }
    }
}
