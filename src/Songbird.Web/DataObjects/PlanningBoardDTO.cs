using System;
using System.Collections.Generic;

namespace Songbird.Web.DataObjects;

public class PlanningBoardDTO : DataObjectBase {
    public DateTime StartDate { get; set; }
    public ICollection<UserScheduleDTO> UserSchedules { get; set; }
    public Int16 WeekNumber { get; set; }
    public Int16 Year { get; set; }
}
