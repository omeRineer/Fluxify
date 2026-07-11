using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class ChatService : IChatService
    {
        readonly IChatRepository _chatRepository;
        readonly IChatUserRepository _chatUserRepository;

        public ChatService(IChatRepository chatRepository, IChatUserRepository chatUserRepository)
        {
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
        }

        public async Task<IResult> CreateAsync(Guid nameSpaceId, CreateChat.Request req)
        {
            var chatId = Guid.NewGuid();

            var newChat = new Chat
            {
                Id = chatId,
                NameSpaceId = nameSpaceId
            };

            await _chatRepository.AddAsync(newChat);

            var chatUsers = req.UserIds.Select(userId => new ChatUser
            {
                ChatId = chatId,
                UserId = userId
            }).ToList();

            await _chatUserRepository.AddRangeAsync(chatUsers);

            return new SuccessResult("Chat is created");
        }
    }
}
