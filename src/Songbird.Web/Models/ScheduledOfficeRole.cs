using System;

namespace Songbird.Web.Models;

public class ScheduledOfficeRole : ModelBase {
    public Guid ScheduleId { get; set; }
    public UserSchedule Schedule { get; set; }

    public OfficeRole Role { get; set; }
}
