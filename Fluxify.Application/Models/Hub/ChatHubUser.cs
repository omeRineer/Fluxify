namespace Fluxify.Application.Models.Hub
{
    public class ChatHubUser : HubUser
    {
        public List<Guid>? Chats { get; set; }
    }

}
