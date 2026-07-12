using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Domain.Enums;

namespace Fluxify.Application.Repositories
{
    public interface IUserNotificationRepository : IRepository<UserNotification, Guid>
    {
        Task MarkAsStatusAsync(NotificationStatus status, Guid[] notifications, Guid[]? users = null);
        Task<List<UserNotification>> GetListAsync(Guid notificationId);
    }
}
