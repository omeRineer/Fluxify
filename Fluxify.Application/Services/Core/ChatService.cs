using AutoMapper;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Core
{
    public class ChatService : IChatService
    {
        readonly IChatRepository _chatRepository;
        readonly IChatUserRepository _chatUserRepository;
        readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IChatUserRepository chatUserRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<Guid>> CreateAsync(Guid nameSpaceId, Guid userId, CreateChat.Request req)
        {
            var newChat = new Chat
            {
                Id = Guid.NewGuid(),
                NameSpaceId = nameSpaceId,
                CreatedUserId = userId,
                Users = req.UserIds.Append(userId).Select(s => new ChatUser
                {
                    UserId = s
                }).ToList(),
                Type = ChatType.Direct,
                Title = req.Title
            };

            await _chatRepository.AddAsync(newChat);
            await _chatRepository.SaveChangesAsync();

            return new SuccessDataResult<Guid>(newChat.Id);
        }

        public async Task<IDataResult<DataListResponse<ChatDto.List>>> GetListAsync(Guid nameSpaceId, Guid userId)
        {
            var chatList = await _chatRepository.GetListAsync(f => f.Users.Any(x => x.UserId == userId));

            var mappedList = _mapper.Map<List<ChatDto.List>>(chatList);

            return new SuccessDataResult<DataListResponse<ChatDto.List>>(DataListResponse<ChatDto.List>.Load(mappedList));

        }
    }
}
