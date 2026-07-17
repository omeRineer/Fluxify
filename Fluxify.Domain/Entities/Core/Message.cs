namespace Fluxify.Domain.Core
{
    public class Message : MongoEntity<Guid>
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}
