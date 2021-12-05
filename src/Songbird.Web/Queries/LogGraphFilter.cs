namespace Songbird.Web.Queries {
    public class LogGraphFilter : SearchFilterBase {
        public string[] Application { get; set; }
        public string[] Level { get; set; }
        public DateWithRange DateRange { get; set; }
    }
}
