using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Repositories;
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
        readonly IChatService _chatService;
        readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository, IChatService chatService)
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
        }

    }
}
