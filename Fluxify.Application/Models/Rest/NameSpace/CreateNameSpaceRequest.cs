namespace Fluxify.Application.Models.Rest.NameSpace
{
    public class CreateNameSpaceRequest
    {
        public string Signature { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
