using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IDataResult<GetNotificationsResponse>> GetListAsync();
        Task<IDataResult<GetNotificationDetailResponse>> GetDetailAsync(Guid id);
        Task<IResult> CreateAsync(CreateNotificationRequest req);
    }
}
