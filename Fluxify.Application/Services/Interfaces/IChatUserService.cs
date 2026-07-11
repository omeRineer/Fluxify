using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IChatUserService
    {
        Task<IResult> AddUsersAsync(Guid chatId, Guid[] userIds);
    }
}
