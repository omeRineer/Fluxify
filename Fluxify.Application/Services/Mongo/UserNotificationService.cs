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
using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Repositories.Mongo;


namespace Fluxify.Application.Services.Mongo
{
    public class UserNotificationService : IUserNotificationService
    {
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly IMapper _mapper;

        public UserNotificationService(IUserNotificationRepository userNotificationRepository, IMapper mapper)
        {
            UserNotificationRepository = userNotificationRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(Guid userId, Guid[] notificationIds)
        {
            await UserNotificationRepository.InsertManyAsync(notificationIds.Select(s => new UserNotification
            {
                NotificationId = s,
                UserId = userId
            }));

            return new SuccessResult();
        }

    }
}
