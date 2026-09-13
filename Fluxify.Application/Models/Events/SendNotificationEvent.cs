namespace Fluxify.Application.Models.Events
{
    public class SendNotificationEvent
    {
        public Guid? Id { get; set; }
        public Guid NameSpaceId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime Date { get; set; }
        public List<Guid>? Users { get; set; }
        public List<string>? Topics { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
    }
}
