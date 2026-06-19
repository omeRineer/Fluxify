using FirebaseAdmin.Messaging;
using Fluxify.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Infrastructure.Services
{
    public class FirebaseNotificationService : INotificationService
    {
        public async Task<string> SendNotificationAsync(string deviceToken, string title, string summary, Dictionary<string, string>? data = null)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = summary
                },
                Data = data
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<IEnumerable<string>> SendNotificationAsync(IEnumerable<string> deviceTokens, string title, string summary, Dictionary<string, string>? data = null)
        {
            var message = new MulticastMessage()
            {
                Tokens = deviceTokens.ToList(),
                Notification = new Notification()
                {
                    Title = title,
                    Body = summary
                },
                Data = data
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

            return response.Responses.Where(f => f.IsSuccess).Select(s => s.MessageId);
        }

        public async Task<string> SendToTopicAsync(string topic, string title, string summary, Dictionary<string, string>? data = null)
        {
            var message = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = summary
                },
                Data = data
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
