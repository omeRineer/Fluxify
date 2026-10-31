namespace Fluxify.Application.Models.Dto.User
{
    public partial class UserDto
    {
        public class Profile
        {
            public Guid Id { get; set; }
            public Guid NameSpaceId { get; set; }
            public string Name { get; set; }
            public string? Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Key { get; set; }
            public DateTime AccountDate { get; set; }
            public DateTime? BirthdayDate { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }
    }

}
