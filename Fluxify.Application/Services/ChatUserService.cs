using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class ChatUserService : IChatUserService
    {
        public Task<IResult> AddUsersAsync(Guid chatId, Guid[] userIds)
        {
            throw new NotImplementedException();
        }
    }
}
