using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ApiDataResponse<GetNotificationsResponse>> GetListAsync();
        Task<ApiDataResponse<GetNotificationDetailResponse>> GetDetailAsync(Guid id);
        Task<ApiResponse> CreateAsync(CreateNotificationRequest req);
    }
}
