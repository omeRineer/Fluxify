namespace Fluxify.Application.Models.SignalR
{
    public class PushToClientUserDisconnectedInfoModel
    {
        public string UserId { get; set; }
        public DateTime DisconnectedDate { get; set; }
    }
}
