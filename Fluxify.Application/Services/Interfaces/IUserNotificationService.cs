using Fluxify.Application.Models.Dto.UserNotification;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Notification;
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
    public interface IUserNotificationService
    {
        Task<IResult> MarkAsStatusAsync(NotificationStatus status, Guid[] notificationIds, Guid[]? userIds = null);

        #region Internal
        Task<IDataResult<List<UserNotification>>> GetTargetsAsync(Guid notificationId);
        #endregion
    }
}
