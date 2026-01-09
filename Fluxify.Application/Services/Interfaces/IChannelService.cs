using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IChannelService
    {
        Task<IDataResult<Channel>> GetAsync(Guid id);

        Task<IDataResult<CreateChannelResponse>> CreateAsync(CreateChannelRequest model);
        Task<IDataResult<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model);
        Task<IDataResult<GetChannelDetailResponse>> GetDetailAsync(Guid id);
    }
}
