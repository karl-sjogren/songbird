using System;
using System.Collections.Generic;

namespace Songbird.Web.Models;

public class FikaSchedule : ModelBase {
    public FikaSchedule() {
        Matches = new List<FikaMatch>();
    }

    public DateTime StartDate { get; set; }
    public ICollection<FikaMatch> Matches { get; set; }
}
