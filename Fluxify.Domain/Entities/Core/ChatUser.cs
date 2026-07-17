namespace Fluxify.Domain.Entities.Core
{
    public class ChatUser : CoreEntity<Guid>
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual User User { get; set; }
    }
}
