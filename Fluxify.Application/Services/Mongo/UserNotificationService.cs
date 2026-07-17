using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
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
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Application.Services.Interfaces.Mongo;


namespace Fluxify.Application.Services.Mongo
{
    public class UserNotificationService
    {
        //readonly IUserNotificationRepository UserNotificationRepository;
        //readonly IMapper _mapper;

        //public UserNotificationService(IUserNotificationRepository userNotificationRepository, IMapper mapper)
        //{
        //    UserNotificationRepository = userNotificationRepository;
        //    _mapper = mapper;
        //}

        //public async Task<IResult> AddAsync(Guid userId, Guid notificationId)
        //{
        //    await UserNotificationRepository.AddAsync(new UserNotification
        //    {
        //        NotificationId = notificationId,
        //        UserId = userId
        //    });

        //    return new SuccessResult();
        //}

        //public async Task<IResult> MarkAsStatusAsync(NotificationStatus status, Guid[] notificationIds, Guid[]? userIds = null)
        //{
        //    await UserNotificationRepository.MarkAsStatusAsync(status, notificationIds, userIds);

        //    return new SuccessResult();
        //}
    }
}
