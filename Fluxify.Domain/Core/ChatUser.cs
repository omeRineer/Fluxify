namespace Fluxify.Domain.Core
{
    public class ChatUser : MongoEntity<Guid>
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
    }
}
