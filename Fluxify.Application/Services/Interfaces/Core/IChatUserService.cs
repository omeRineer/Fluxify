using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IChatUserService
    {
        Task<List<Guid>> GetChatsByUser(Guid userId);
    }
}
