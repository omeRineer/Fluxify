using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces.Mongo
{
    public interface IUserNotificationService
    {
        Task<IResult> MarkAsReadAsync(Guid userId, Guid notificationId);

    }
}
