using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Dto.Message;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces.Mongo
{
    public interface IMessageService
    {
        Task<IResult> SendAsync(Guid senderId, Guid nameSpaceId, Guid chatId, SendMessage.Request req);
        Task<IDataResult<DataListResponse<MessageDto.List>>> GetHistoryAsync(Guid userId, Guid chatId);

    }
}
