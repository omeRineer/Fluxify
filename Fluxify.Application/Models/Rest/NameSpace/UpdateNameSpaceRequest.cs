namespace Fluxify.Application.Models.Rest.NameSpace
{
    public class UpdateNameSpaceRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
