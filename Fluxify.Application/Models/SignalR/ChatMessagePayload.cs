namespace Fluxify.Application.Models.SignalR
{
    public class ChatMessagePayload
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
