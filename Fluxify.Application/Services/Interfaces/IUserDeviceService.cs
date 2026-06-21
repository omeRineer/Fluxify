using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IUserDeviceService
    {
        Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserDevice.Request req);
        Task<IDataResult<List<UserDevice>>> GetListAsync(Guid[] userIds);

    }
}
