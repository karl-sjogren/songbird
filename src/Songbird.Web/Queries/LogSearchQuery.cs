namespace Songbird.Web.Queries {
    public class LogSearchQuery : SearchQueryBase {
        public string Query { get; set; }

        public override bool IsEmpty => string.IsNullOrWhiteSpace(Query);
    }
}
