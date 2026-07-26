namespace Fluxify.Application.Models.Events
{
    public class SendNotificationEvent
    {
        public Guid? Id { get; set; }
        public Guid NameSpaceId { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime Date { get; set; }
        public string? Url { get; set; }
        public List<string> Topics { get; set; }
    }
}
