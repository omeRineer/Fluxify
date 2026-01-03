using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using Fluxify.Domain.Enums;

namespace Fluxify.Application.Repositories
{
    public interface IUserNotificationRepository : IMongoRepository<UserNotification, Guid>
    {
        public Task MarkAsStatusAsync(NotificationStatus status, params Guid[] ids);
        public Task MarkAsStatusByNotificationAsync(NotificationStatus status, Guid? userId = null, params Guid[] notificationIds);
    }
}
