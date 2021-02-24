namespace Songbird.Web.Models {
    public record SlackMessage {
        public string Webhook { get; init; }
        public string Message { get; init; }
    }
}
