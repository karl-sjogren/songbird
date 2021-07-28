using System;
using System.Collections.Generic;

namespace Songbird.Web.Models {
    public class Application : ModelBase {
        public Application() {
            LogFilters = new List<ApplicationLogFilter>();
        }

        public string Name { get; set; }

        public byte[] Timestamp { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<ApplicationLogFilter> LogFilters { get; set; }
    }
}
