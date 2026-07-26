using AutoMapper;
using Fluxify.Application.Infrastructure;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Dto.Message;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Mongo
{
    public class MessageService : IMessageService
    {
        readonly IEventPublisher _eventPublisher;
        readonly IMessageRepository _messageRepository;
        readonly IChatRepository _chatRepository;
        readonly IChatUserRepository _chatUserRepository;
        readonly IMapper _mapper;

        public MessageService(IEventPublisher eventPublisher, IMessageRepository messageRepository, IMapper mapper, IChatRepository chatRepository, IChatUserRepository chatUserRepository)
        {
            _eventPublisher = eventPublisher;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
        }

        public async Task<IDataResult<DataListResponse<MessageDto.List>>> GetHistoryAsync(Guid nameSpaceId, Guid chatId)
        {
            var chatUsers = await _chatUserRepository.GetProjectedListAsync(s => new { s.UserId, s.User.Name }, f => f.ChatId == chatId);
            var chatHistory = await _messageRepository.GetListAsync(f => f.ChatId == chatId);

            var mappedList = _mapper.Map<List<MessageDto.List>>(chatHistory);
            mappedList.ForEach(item => item.Name = chatUsers.First(f => f.UserId == item.UserId).Name);

            return new SuccessDataResult<DataListResponse<MessageDto.List>>(DataListResponse<MessageDto.List>.Load(mappedList));
        }

        public async Task<IResult> SendAsync(Guid senderId, Guid nameSpaceId, Guid chatId, SendMessage.Request req)
        {
            await _eventPublisher.PublishAsync("chat-direct",
            new SendMessageEvent
            {
                UserId = senderId,
                NameSpaceId = nameSpaceId,
                ChatId = chatId,
                Text = req.Text
            },
            "message.send");

            return new SuccessResult("Message is sent");
        }
    }
}
