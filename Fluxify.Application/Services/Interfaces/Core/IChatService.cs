using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IChatService
    {
        Task<IDataResult<ChatDto.Detail>> CreateAsync(Guid nameSpaceId, Guid userId, CreateChat.Request req);
        Task<IDataResult<DataListResponse<ChatDto.List>>> GetListAsync(Guid nameSpaceId, Guid userId);
        Task<IDataResult<ChatDto.Detail>> GetDetailAsync(Guid userId, Guid chatId);
    }
}
