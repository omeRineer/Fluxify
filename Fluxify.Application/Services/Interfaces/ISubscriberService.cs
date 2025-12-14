using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Utilities.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface ISubscriberService
    {
        Task<ApiDataResponse<CreateSubscriberResponse>> CreateAsync(CreateSubscriberRequest model);
        Task<ApiDataResponse<UpdateSubscriberResponse>> UpdateAsync(UpdateSubscriberRequest model);
        Task<ApiDataResponse<GetSubscriberDetailResponse>> GetDetailAsync(Guid id);
        Task<ApiDataResponse<GetSubscriberDetailResponse?>> GetDetailAsync(string apiKey);
        Task<ApiDataResponse<GetSubscriberListResponse>> GetSubscribersByNameSpaceAsync(Guid nameSpace);

    }
}
