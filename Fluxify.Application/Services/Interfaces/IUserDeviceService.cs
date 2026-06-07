using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IUserDeviceService
    {
        Task<IResult> SetAsync(Guid userId, SetUserDevice.Request req);

    }
}
