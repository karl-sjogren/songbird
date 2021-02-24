namespace Songbird.Web.Models {
    public record SlackMessageWithLinkButton : SlackMessage {
        public string ButtonText { get; init; }
        public string ButtonUrl { get; init; }
    }
}
