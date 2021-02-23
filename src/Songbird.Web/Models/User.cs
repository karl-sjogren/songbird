using System;
using System.Collections.Generic;

namespace Songbird.Web.Models {
    public class User : ModelBase {
        public User() {
            FikaMatches = new List<FikaMatch>();
        }

        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsEligibleForFikaScheduling { get; set; }
        public DateTime? LastLogin { get; set; }
        public ICollection<FikaMatch> FikaMatches { get; set; }
    }
}
