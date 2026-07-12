using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Utilities.Result;


namespace Fluxify.Application.Services.Interfaces
{
    public interface IUserDeviceService
    {
        Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserDevice.Request req);
        Task<IDataResult<DataListResponse<UserDeviceDto.List>>> GetListAsync(Guid userId);

    }
}
