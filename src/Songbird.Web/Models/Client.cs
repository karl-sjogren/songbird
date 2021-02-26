using System.Collections.Generic;

namespace Songbird.Web.Models {
    public class Customer : ModelBase {
        public Customer() {
            Projects = new List<Project>();
        }

        public string Name { get; set; }
        public byte[] Timestamp { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
