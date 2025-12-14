using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Utilities.ApiResponse;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IChannelService
    {
        Task<ApiDataResponse<CreateChannelResponse>> CreateAsync(CreateChannelRequest model);
        Task<ApiDataResponse<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model);
        Task<ApiDataResponse<GetChannelDetailResponse>> GetDetailAsync(Guid id);
    }
}
