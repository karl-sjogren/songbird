using System;

namespace Songbird.Web.DataObjects;

public class ScheduledStatusDTO : DataObjectBase {
    public Guid ScheduleId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public OfficeStatusDTO MorningStatus { get; set; }
    public OfficeStatusDTO AfternoonStatus { get; set; }
}
