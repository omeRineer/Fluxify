using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
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
            var newChat = new Chat
            {
                NameSpaceId = nameSpaceId,
                Users = req.UserIds.Select(userId => new ChatUser
                {
                    UserId = userId
                }).ToList()
            };

            await _chatRepository.AddAsync(newChat);

            return new SuccessResult("Chat is created");
        }
    }
}
