using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using Fluxify.Domain.Enums;

namespace Fluxify.Application.Repositories
{
    public interface IUserNotificationRepository : IMongoRepository<UserNotification, Guid>
    {
        Task MarkAsStatusAsync(NotificationStatus status, Guid[] notifications, Guid[]? users = null);
        Task<List<UserNotification>> GetListAsync(Guid notificationId);
    }
}
