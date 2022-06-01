using System;

namespace Songbird.Web.Models;

public class ScheduledStatus : ModelBase {
    public Guid ScheduleId { get; set; }
    public UserSchedule Schedule { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public OfficeStatus MorningStatus { get; set; }
    public OfficeStatus AfternoonStatus { get; set; }
}
