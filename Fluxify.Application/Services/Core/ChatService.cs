using AutoMapper;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IDataResult<ChatDto.Detail>> CreateAsync(Guid nameSpaceId, Guid userId, CreateChat.Request req)
        {
            var mappedEntity = _mapper.Map<Chat>(req);
            mappedEntity.Users.Add(new ChatUser { UserId = userId });
            mappedEntity.NameSpaceId = nameSpaceId;
            mappedEntity.CreatedUserId = userId;

            await _chatRepository.AddAsync(mappedEntity);
            await _chatRepository.SaveChangesAsync();

            var mappedResult = _mapper.Map<ChatDto.Detail>(mappedEntity);

            return new SuccessDataResult<ChatDto.Detail>(mappedResult);
        }

        public async Task<IDataResult<ChatDto.Detail>> GetDetailAsync(Guid userId, Guid chatId)
        {
            var chat = await _chatRepository.GetSingleOrDefaultAsync(f => f.Users.Any(x => x.UserId == userId) && f.Id == chatId,
                                                                     i => i.Include(x => x.CreatedUser).Include(i => i.Users));

            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<Chat>(chat)
            );

            var mappedEntity = _mapper.Map<ChatDto.Detail>(chat);

            return new SuccessDataResult<ChatDto.Detail>(mappedEntity);
        }

        public async Task<IDataResult<DataListResponse<ChatDto.List>>> GetListAsync(Guid nameSpaceId, Guid userId)
        {
            var chatList = await _chatRepository.GetListAsync(f => f.Users.Any(x => x.UserId == userId));

            var mappedList = _mapper.Map<List<ChatDto.List>>(chatList);

            return new SuccessDataResult<DataListResponse<ChatDto.List>>(DataListResponse<ChatDto.List>.Load(mappedList));

        }
    }
}
