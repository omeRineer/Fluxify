using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Infrastructure
{
    public interface IPushNotificationService
    {
        Task<string> SendNotificationAsync(string deviceToken, string title, string summary, string? thumbnail = null, Dictionary<string, string>? data = null);
        Task<IEnumerable<string>> SendNotificationAsync(IEnumerable<string> deviceTokens, string title, string summary, string? thumbnail = null, Dictionary<string, string>? data = null);
        Task<string> SendToTopicAsync(string topic, string title, string summary, string? thumbnail = null, Dictionary<string, string>? data = null);
    }
}
