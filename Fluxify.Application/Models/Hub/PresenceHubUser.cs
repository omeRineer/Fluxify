using Fluxify.Application.Models.Enums;
using Fluxify.Application.Utilities.Helpers;
using System.Globalization;

namespace Fluxify.Application.Models.Hub
{
    public class PresenceHubUser : HubUser
    {
        public string Status { get; private set; }

        public PresenceHubUser()
        {
            Status = EnumHelper.ToEnumString<UserAvailabilityStatus>((int)UserAvailabilityStatus.Online);
        }

        public void SetStatus(string status)
        {
            Status = EnumHelper.Parse<UserAvailabilityStatus>(status).ToString().ToLower(new CultureInfo("en-EN"));
        }
    }

}
