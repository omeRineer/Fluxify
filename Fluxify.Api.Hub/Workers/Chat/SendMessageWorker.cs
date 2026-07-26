using AutoMapper;
using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Infrastructure;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Entities.Mongo;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;

namespace Fluxify.Api.Hub.Workers.Chat
{
    public class SendMessageWorker : ConsumerWorker<SendMessageEvent>
    {
        protected override string QueueName => "message-incoming";
        protected override int RetryCount => 0;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<ChatHub> _hubContext;
        readonly HubConnectionManager<PresenceHubUser> _presenceHubConnectionManager;
        readonly HubConnectionManager<ChatHubUser> _chatHubConnectionManager;

        public SendMessageWorker(IChannel channel, IServiceScopeFactory scopeFactory, IHubContext<ChatHub> hubContext, HubConnectionManager<PresenceHubUser> presenceHubConnectionManager, HubConnectionManager<ChatHubUser> chatHubConnectionManager) : base(channel)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _presenceHubConnectionManager = presenceHubConnectionManager;
            _chatHubConnectionManager = chatHubConnectionManager;
        }

        protected override async Task HandleAsync(SendMessageEvent message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
            var _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var chat = await chatRepository.FindAsync(message.ChatId);

            if (chat == null)
                throw new Exception("Chat not found");

            var newMessage = _mapper.Map<Message>(message);
            await messageRepository.InsertAsync(newMessage);

            var senderName = await userRepository.GetProjectedSingleOrDefaultAsync(s => s.Name, f => f.Id == message.UserId);

            await _hubContext.Clients.Group($"chat:{message.ChatId}").SendAsync(HubEvents.ChatHub.ReceiveMessage, new ChatMessagePayload
            {
                ChatId = message.ChatId,
                UserId = message.UserId,
                Name = senderName,
                Text = message.Text,
                Date = message.Date
            });
        }
    }
}
