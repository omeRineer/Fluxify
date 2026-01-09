using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IDataResult<Notification>> GetAsync(Guid id);
        Task<IDataResult<List<UserNotification>>> GetTargetsAsync(Guid notificationId);

        Task<IDataResult<GetNotificationsResponse>> GetListAsync();
        Task<IDataResult<GetNotificationsResponse>> GetAndMarkPendingAsync(Guid userId, Guid nameSpace);
        Task<IDataResult<GetNotificationDetailResponse>> GetDetailAsync(Guid id);
        Task<IResult> CreateAsync(CreateNotificationRequest req);
        Task<IResult> MarkAsStatusByNotificationAsync(NotificationStatus status, Guid? userId = null, params Guid[] notificationIds);
        Task<IResult> MarkAsStatusAsync(NotificationStatus status, params Guid[] ids);
    }
}
