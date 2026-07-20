using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Infrastructure;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Domain.Entities.Core;
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
        readonly HubConnectionManager<PresenceHub> _presenceHubConnectionManager;
        readonly HubConnectionManager<ChatHub> _chatHubConnectionManager;

        public SendMessageWorker(IChannel channel, IServiceScopeFactory scopeFactory, IHubContext<ChatHub> hubContext, HubConnectionManager<PresenceHub> presenceHubConnectionManager, HubConnectionManager<ChatHub> chatHubConnectionManager) : base(channel)
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
            var chatUserRepository = scope.ServiceProvider.GetRequiredService<IChatUserRepository>();
            var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

            //var chat = await chatRepository.FindAsync(message.ChatId);

            //if (chat == null)
            //    throw new Exception("Chat not found");

            //var newMessage = new Domain.Entities.Mongo.Message
            //{
            //    ChatId = message.ChatId,
            //    UserId = message.SenderId,
            //    Text = message.Text
            //};
            //await messageRepository.InsertAsync(newMessage);

            //var chatUsers = await chatUserRepository.GetProjectedListAsync(s => s.User.Id, f => f.ChatId == message.ChatId);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new ChatMessageMessage
            {
                ChatId = message.ChatId,
                UserId = message.SenderId,
                Text = message.Text,
                Date = DateTime.UtcNow
            });
        }
    }
}
