using Fluxify.Application.Models.Common.Pagination;

namespace Fluxify.Application.Models.Rest.Notification
{
    public class NotificationQuery : QueryOptions
    {
        public bool IsRead { get; set; } = false;

        public DateTime LastDate { get; set; } = DateTime.UtcNow;
    }
}
