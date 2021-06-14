using System;
using Newtonsoft.Json;

namespace Songbird.Web.DataObjects {
    public class Log4StashEntry {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "@timestamp")]
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string LoggerName { get; set; }
        public string Level { get; set; }
        [JsonProperty(PropertyName = "log4net:UserName")]
        public string Application { get; set; }
        [JsonProperty(PropertyName = "log4net:HostName")]
        public string Hostname { get; set; }
        [JsonProperty(PropertyName = "log4net:Identity")]
        public string ClientIdentity { get; set; }
    }
}
