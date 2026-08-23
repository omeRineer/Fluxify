namespace Fluxify.Domain.Entities.Core
{
    public class Credential : CoreEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }


        public virtual User User { get; set; }
    }
}
