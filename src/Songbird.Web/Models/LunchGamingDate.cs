using System;
using System.Collections.Generic;

namespace Songbird.Web.Models {
    public class LunchGamingDate : ModelBase {
        public LunchGamingDate() {
            Attendees = new List<LunchGamingAttendance>();
        }

        public DateTime Date { get; set; }
        public Guid GameId { get; set; }
        public LunchGame Game { get; set; }

        public ICollection<LunchGamingAttendance> Attendees { get; set; }
    }
}
