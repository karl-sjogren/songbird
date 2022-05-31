using System;

namespace Songbird.Web.Models;

public class ScheduledStatus : ModelBase {
    public Guid ScheduleId { get; set; }
    public UserSchedule Schedule { get; set; }

    public OfficeStatus Status { get; set; }
}
