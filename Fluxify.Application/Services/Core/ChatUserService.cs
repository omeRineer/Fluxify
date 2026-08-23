using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Core
{
    public class ChatUserService : IChatUserService
    {
        readonly IChatUserRepository _chatUserRepository;

        public ChatUserService(IChatUserRepository chatUserRepository)
        {
            _chatUserRepository = chatUserRepository;
        }

        public async Task<List<Guid>> GetChatsByUser(Guid userId)
        {
            var chats = await _chatUserRepository.GetProjectedListAsync(s => s.ChatId, f => f.UserId == userId);

            return chats;
        }
    }
}
