using Fluxify.Application.Infrastructure;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class MessageService : IMessageService
    {
        readonly IEventPublisher _eventPublisher;

        public MessageService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task<IResult> SendAsync(Guid senderId, Guid nameSpaceId, Guid chatId, SendMessage.Request req)
        {
            await _eventPublisher.PublishAsync("chat-direct",
            new SendMessageEvent
            {
                SenderId = senderId,
                NameSpaceId = nameSpaceId,
                ChatId = chatId,
                Text = req.Text
            },
            "message.send");

            return new SuccessResult("Message is sent");
        }
    }
}
