using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IChannelService
    {
        Task<IDataResult<CreateChannelResponse>> CreateAsync(CreateChannelRequest model);
        Task<IDataResult<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model);
        Task<IDataResult<GetChannelDetailResponse>> GetDetailAsync(Guid id);
    }
}
