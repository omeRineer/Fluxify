using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Utilities.Result;


namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IUserDeviceService
    {
        Task<IResult> RegisterAsync(Guid userId, Guid nameSpaceId, SetUserDevice model);
        Task<IResult> RegisterAsync(string key, Guid nameSpaceId, SetUserDevice model);
        Task<IDataResult<DataListResponse<UserDeviceDto.List>>> GetListAsync(Guid userId);

    }
}
