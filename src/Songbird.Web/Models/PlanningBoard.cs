using System;
using System.Collections.Generic;

namespace Songbird.Web.Models;

public class PlanningBoard : ModelBase {
    public PlanningBoard() {
        UserSchedules = new List<UserSchedule>();
    }

    public DateTime StartDate { get; set; }
    public ICollection<UserSchedule> UserSchedules { get; set; }
    public Int16 WeekNumber { get; set; }
    public Int16 Year { get; set; }
}
