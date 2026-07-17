namespace Fluxify.Application.Models.Events
{
    public class SendMessageEvent
    {
        public Guid SenderId { get; set; }
        public Guid NameSpaceId { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; }
    }
}
