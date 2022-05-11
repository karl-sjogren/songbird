using System;

namespace Songbird.Web.Models;

public class PlannedProjectTime : ModelBase {
    public Guid ScheduleId { get; set; }
    public UserSchedule Schedule { get; set; }

    public double Time { get; set; }
    public byte[] Timestamp { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
}
