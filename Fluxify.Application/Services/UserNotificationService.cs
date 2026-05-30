using Fluxify.Application.Models.Dto.UserNotification;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Common;
using AutoMapper;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        readonly INotificationRepository NotificationRepository;
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly IMapper _mapper;

        public UserNotificationService(IUserNotificationRepository userNotificationRepository, INotificationRepository notificationRepository, IMapper mapper)
        {
            UserNotificationRepository = userNotificationRepository;
            NotificationRepository = notificationRepository;
            _mapper = mapper;
        }        

        public async Task<IDataResult<List<UserNotification>>> GetTargetsAsync(Guid notificationId)
        {
            var userNotifications = await UserNotificationRepository.GetListAsync(f => f.NotificationId == notificationId);

            return new SuccessDataResult<List<UserNotification>>(userNotifications);
        }

        public async Task<IResult> MarkAsStatusAsync(NotificationStatus status, Guid[] notificationIds, Guid[]? userIds = null)
        {
            await UserNotificationRepository.MarkAsStatusAsync(status, notificationIds, userIds);

            return new SuccessResult();
        }
    }
}
