using System;
using System.Collections.Generic;

namespace Songbird.Web.Models;

public class PlaningBoard : ModelBase {
    public PlaningBoard() {
        UserSchedules = new List<UserSchedule>();
    }

    public DateTime StartDate { get; set; }
    public ICollection<UserSchedule> UserSchedules { get; set; }
    public Int32 WeekNumber { get; set; }
    public Int32 Year { get; set; }
    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }
}
