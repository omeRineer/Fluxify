using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IChatService
    {
        Task<IResult> CreateAsync(Guid nameSpaceId, CreateChat.Request req);
    }
}
