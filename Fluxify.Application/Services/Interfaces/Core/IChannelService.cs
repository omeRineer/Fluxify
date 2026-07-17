using Fluxify.Application.Models.Dto.Channel;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IChannelService
    {
        Task<IDataResult<CreateChannel.Response>> CreateAsync(CreateChannel.Request model);
        Task<IDataResult<UpdateChannel.Response>> UpdateAsync(Guid id, UpdateChannel.Request model);
        Task<IDataResult<ChannelDto.Detail>> GetDetailAsync(Guid id);
    }
}
