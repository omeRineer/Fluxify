namespace Fluxify.Application.Models.Rest.Notification
{
    public class FireNotification
    {
        public class  Request
        {
            public string? Type { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string? Url { get; set; }
            public string? Thumbnail { get; set; }
            public Target? To { get; set; }

            public class Target
            {
                public List<string> Topics { get; set; } = new();
                public List<string> Users { get; set; } = new();
            }
        }
    }
}
