using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface ISubscriberService
    {
        Task<IDataResult<CreateSubscriberResponse>> CreateAsync(CreateSubscriberRequest model);
        Task<IDataResult<UpdateSubscriberResponse>> UpdateAsync(UpdateSubscriberRequest model);
        Task<IDataResult<GetSubscriberDetailResponse>> GetDetailAsync(Guid id);
        Task<IDataResult<GetSubscriberDetailResponse?>> GetDetailAsync(string apiKey);
        Task<IDataResult<List<SubscriberResponse>>> GetListByNameSpaceAsync(Guid nameSpace);

    }
}
