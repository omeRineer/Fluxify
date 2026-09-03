using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Models.Dto.Notification;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Repositories;
using Fluxify.Domain.Entities.Mongo;

namespace Fluxify.Application.Services.Interfaces.Mongo
{
    public interface INotificationService
    {
        Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid nameSpaceId, Guid userId, Guid id);
        Task<IDataResult<DataListResponse<NotificationDto.List>>> GetListAsync(Guid nameSpaceId, Guid userId);
        Task<IResult> CreateAsync(Guid nameSpaceId, CreateNotification.Request req);
    }
}
