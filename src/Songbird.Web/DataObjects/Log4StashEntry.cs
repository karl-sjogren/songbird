using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Songbird.Web.DataObjects {
    // These are annotated with both Newtonsoft.Json and System.Text.Json
    // since they are deserialized by Elasticsearch.Net using Newtonsoft.Json
    // and then passed to the api using System.Text.Json.
    public class Log4StashEntry {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "@timestamp")]
        [JsonPropertyName("@timestamp")]
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string LoggerName { get; set; }
        public string Level { get; set; }
        [JsonProperty(PropertyName = "log4net:UserName")]
        [JsonPropertyName("log4net:UserName")]
        public string Application { get; set; }
        [JsonProperty(PropertyName = "log4net:HostName")]
        [JsonPropertyName("log4net:HostName")]
        public string Hostname { get; set; }
        [JsonProperty(PropertyName = "log4net:Identity")]
        [JsonPropertyName("log4net:Identity")]
        public string ClientIdentity { get; set; }
    }
}
