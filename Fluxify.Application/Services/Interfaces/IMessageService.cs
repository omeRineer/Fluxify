using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IMessageService
    {
       Task<IResult> SendAsync(Guid senderId, Guid nameSpaceId, Guid chatId, SendMessage.Request req);
    }
}
