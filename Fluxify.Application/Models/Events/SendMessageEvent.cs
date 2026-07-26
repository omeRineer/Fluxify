namespace Fluxify.Application.Models.Events
{
    public class SendMessageEvent
    {
        public Guid UserId { get; set; }
        public Guid NameSpaceId { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; } = DateTime.UtcNow;
    }
}
