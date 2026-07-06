namespace Fluxify.Application.Models.Queue
{
    public class NotificationQueueModel
    {
        public Guid? Id { get; set; }
        public Guid NameSpaceId { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string? Image { get; set; }
        public List<string> Topics { get; set; }
    }
}
