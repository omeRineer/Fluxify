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

namespace Fluxify.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid id, Guid userId);
        Task<IDataResult<DataListResponse<NotificationDto.List>>> GetListAsync(Guid userId);
        Task<IResult> CreateAsync(Guid nameSpaceId, Guid userId, CreateNotification.Request req);
        Task<IResult> FireAsync(Guid nameSpaceId, FireNotification.Request req);

        #region Internal
        Task<IDataResult<Notification>> GetAsync(Guid id);
        #endregion
    }
}
